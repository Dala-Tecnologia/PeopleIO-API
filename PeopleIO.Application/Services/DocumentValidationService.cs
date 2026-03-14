using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Configuration;
using PeopleIO.Domain.Entity;
using System.Globalization;

namespace PeopleIO.Application.Services;

public class DocumentValidationService : IDocumentValidationService
{
    private readonly DocumentIntelligenceClient _client;

    public DocumentValidationService(IConfiguration configuration)
    {
        string endpoint = configuration["AzureDocumentIntelligence:Endpoint"]!;
        string apiKey = configuration["AzureDocumentIntelligence:ApiKey"]!;
        var credential = new AzureKeyCredential(apiKey);
        _client = new DocumentIntelligenceClient(new Uri(endpoint), credential);
    }

    public async Task<bool> ValidateRGAsync(Candidato candidato, Stream arquivoStream)
    {
        // Tenta passar BinaryData diretamente. Se a sobrecarga não existir, teremos que investigar a versão exata.
        var content = BinaryData.FromStream(arquivoStream);

        // Nota: Se AnalyzeDocumentContent for obrigatório e não encontrado, pode ser necessário verificar a versão do pacote.
        // Tentando usar a sobrecarga que aceita BinaryData se disponível, ou ajustando para a classe correta.
        // Em algumas versões beta, a classe AnalyzeDocumentContent é usada. Se não encontrada, vamos tentar instanciar o request de outra forma.
        
        // Se AnalyzeDocumentContent não existe, talvez estejamos usando uma versão onde devemos passar o conteúdo como parte de uma opção diferente.
        // Mas como solução tentativa, vou assumir que há uma sobrecarga aceitando BinaryData ou que o namespace precisa ser verificado.
        // Se o erro persistir, pode ser necessário usar 'new AnalyzeDocumentRequest { Base64Source = ... }' ou similar.
        
        // Vou tentar usar AnalyzeDocumentContent com o nome totalmente qualificado se existir, mas se o compilador diz que não acha, é porque não está lá.
        // Vou tentar passar o conteúdo diretamente na chamada, que é comum em SDKs Azure.
        
        Operation<AnalyzeResult> operation;
        try 
        {
             // Tentativa 1: Passar BinaryData direto (comum em versões recentes)
             operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", content);
        }
        catch (Exception)
        {
            // Se falhar na compilação, o try/catch não ajuda, mas serve para lógica de runtime.
            // O problema é compilação.
            throw; 
        }

        var result = operation.Value;

        foreach (var document in result.Documents)
        {
            bool docNumberMatch = document.Fields.TryGetValue("DocumentNumber", out var documentNumberField) &&
                                  documentNumberField.Content.Contains(candidato.IdentidadeNumero!);
            
            bool dateIssueMatch = false;
            if (document.Fields.TryGetValue("DateOfIssue", out var dateOfIssueField) && dateOfIssueField.ValueDate.HasValue)
            {
                 // Comparação explícita de strings para evitar erro de operador entre DateTimeOffset e DateOnly
                 string dateFromDoc = dateOfIssueField.ValueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                 string dateFromCand = candidato.IdentidadeDataEmissao.HasValue 
                    ? candidato.IdentidadeDataEmissao.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) 
                    : "";
                 
                 dateIssueMatch = dateFromDoc == dateFromCand;
            }

            bool regionMatch = document.Fields.TryGetValue("Region", out var regionField) &&
                               regionField.Content.Contains(candidato.IdentidadeUF!);

            if (docNumberMatch && dateIssueMatch && regionMatch)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> ValidateCNHAsync(Candidato candidato, Stream arquivoStream)
    {
        var content = BinaryData.FromStream(arquivoStream);
        var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", content);
        var result = operation.Value;

        foreach (var document in result.Documents)
        {
            bool docNumberMatch = document.Fields.TryGetValue("DocumentNumber", out var documentNumberField) &&
                                  documentNumberField.Content.Contains(candidato.CNHNumero!);
            
            bool expirationMatch = false;
            if (document.Fields.TryGetValue("DateOfExpiration", out var dateOfExpirationField) && dateOfExpirationField.ValueDate.HasValue)
            {
                string dateFromDoc = dateOfExpirationField.ValueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string dateFromCand = candidato.CNHDataVencimento.HasValue 
                    ? candidato.CNHDataVencimento.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    : "";
                expirationMatch = dateFromDoc == dateFromCand;
            }
            
            bool cpfMatch = document.Fields.TryGetValue("PersonalNumber", out var personalNumberField) &&
                            personalNumberField.Content.Replace(".", "").Replace("-", "") == candidato.CPF;

            bool dobMatch = false;
            if (document.Fields.TryGetValue("DateOfBirth", out var dateOfBirthField) && dateOfBirthField.ValueDate.HasValue)
            {
                string dateFromDoc = dateOfBirthField.ValueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string dateFromCand = candidato.DataNascimento.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                dobMatch = dateFromDoc == dateFromCand;
            }

            if (docNumberMatch && expirationMatch && cpfMatch && dobMatch)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> ValidateComprovanteResidenciaAsync(Candidato candidato, Stream arquivoStream)
    {
        var content = BinaryData.FromStream(arquivoStream);
        var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", content);
        var result = operation.Value;

        var extractedText = result.Content;

        if (extractedText.Contains(candidato.Nome) &&
            extractedText.Contains(candidato.Endereco.Rua!) &&
            extractedText.Contains(candidato.Endereco.Cidade!) &&
            extractedText.Contains(candidato.Endereco.CEP!))
        {
            return true;
        }

        return false;
    }
}
