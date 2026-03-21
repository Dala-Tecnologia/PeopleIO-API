using Azure;
using Azure.AI.DocumentIntelligence;
using System.Globalization;

namespace PeopleIO.Application.Services;

public class DocumentValidationService : IDocumentValidationService
{
    private readonly DocumentIntelligenceClient _client;

    public DocumentValidationService(DocumentIntelligenceClient client)
    {
        _client = client;
    }

    // --- Overloads com Candidato (usados no RegisterCandidatoService) ---

    public Task<bool> ValidateRGAsync(global::PeopleIO.Domain.Entity.Candidato candidato, Stream arquivoStream)
        => ValidateRGAsync(candidato.IdentidadeNumero!, candidato.IdentidadeUF!, candidato.IdentidadeDataEmissao, arquivoStream);

    public Task<bool> ValidateCNHAsync(global::PeopleIO.Domain.Entity.Candidato candidato, Stream arquivoStream)
        => ValidateCNHAsync(candidato.CNHNumero!, candidato.CPF, candidato.DataNascimento, candidato.CNHDataVencimento, arquivoStream);

    public Task<bool> ValidateComprovanteResidenciaAsync(global::PeopleIO.Domain.Entity.Candidato candidato, Stream arquivoStream)
        => ValidateComprovanteResidenciaAsync(candidato.Nome, candidato.Endereco.Rua!, candidato.Endereco.Cidade!, candidato.Endereco.CEP!, arquivoStream);

    // --- Overloads com parâmetros individuais (usados nos endpoints de validação avulsa) ---

    public async Task<bool> ValidateRGAsync(string identidadeNumero, string identidadeUF, DateTime? dataEmissao, Stream arquivoStream)
    {
        var result = await AnalyzeAsync("prebuilt-idDocument", arquivoStream);

        foreach (var document in result.Documents)
        {
            bool docNumberMatch = document.Fields.TryGetValue("DocumentNumber", out var documentNumberField) &&
                                  (documentNumberField.Content?.Contains(identidadeNumero) ?? false);

            bool dateIssueMatch = false;
            if (document.Fields.TryGetValue("DateOfIssue", out var dateOfIssueField) && dateOfIssueField.ValueDate.HasValue)
            {
                string dateFromDoc = dateOfIssueField.ValueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string dateFromReq = dataEmissao.HasValue
                    ? dataEmissao.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    : "";
                dateIssueMatch = dateFromDoc == dateFromReq;
            }

            bool regionMatch = document.Fields.TryGetValue("Region", out var regionField) &&
                               (regionField.Content?.Contains(identidadeUF) ?? false);

            if (docNumberMatch && dateIssueMatch && regionMatch)
                return true;
        }

        return false;
    }

    public async Task<bool> ValidateCNHAsync(string cnhNumero, string cpf, DateTime dataNascimento, DateTime? dataVencimento, Stream arquivoStream)
    {
        var result = await AnalyzeAsync("prebuilt-idDocument", arquivoStream);

        foreach (var document in result.Documents)
        {
            bool docNumberMatch = document.Fields.TryGetValue("DocumentNumber", out var documentNumberField) &&
                                  (documentNumberField.Content?.Contains(cnhNumero) ?? false);

            bool expirationMatch = false;
            if (document.Fields.TryGetValue("DateOfExpiration", out var dateOfExpirationField) && dateOfExpirationField.ValueDate.HasValue)
            {
                string dateFromDoc = dateOfExpirationField.ValueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string dateFromReq = dataVencimento.HasValue
                    ? dataVencimento.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    : "";
                expirationMatch = dateFromDoc == dateFromReq;
            }

            bool cpfMatch = document.Fields.TryGetValue("PersonalNumber", out var personalNumberField) &&
                            personalNumberField.Content?.Replace(".", "").Replace("-", "") == cpf;

            bool dobMatch = false;
            if (document.Fields.TryGetValue("DateOfBirth", out var dateOfBirthField) && dateOfBirthField.ValueDate.HasValue)
            {
                string dateFromDoc = dateOfBirthField.ValueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string dateFromReq = dataNascimento.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                dobMatch = dateFromDoc == dateFromReq;
            }

            if (docNumberMatch && expirationMatch && cpfMatch && dobMatch)
                return true;
        }

        return false;
    }

    public async Task<bool> ValidateComprovanteResidenciaAsync(string nome, string rua, string cidade, string cep, Stream arquivoStream)
    {
        var result = await AnalyzeAsync("prebuilt-layout", arquivoStream);

        return result.Content.Contains(nome) &&
               result.Content.Contains(rua) &&
               result.Content.Contains(cidade) &&
               result.Content.Contains(cep);
    }

    private async Task<AnalyzeResult> AnalyzeAsync(string modelId, Stream stream)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        var request = new AnalyzeDocumentContent { Base64Source = BinaryData.FromBytes(ms.ToArray()) };
        var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, modelId, request);
        return operation.Value;
    }
}
