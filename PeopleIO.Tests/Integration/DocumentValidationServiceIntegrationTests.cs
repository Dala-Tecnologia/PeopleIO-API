using Azure;
using Azure.AI.DocumentIntelligence;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using PeopleIO.Application.Services;
using PeopleIO.Domain.Enums;
using Endereco = PeopleIO.Domain.Entity.Endereco;
using Xunit;

namespace PeopleIO.Tests.Integration;

/// <summary>
/// Testes de integração que fazem chamadas reais ao Azure Document Intelligence.
///
/// PRÉ-REQUISITOS:
///   1. Crie o arquivo appsettings.IntegrationTests.json (baseado no .example ao lado)
///      com suas credenciais reais.
///   2. Crie a pasta TestDocuments/ e adicione os arquivos de documento de teste.
///
/// COMO RODAR:
///   Todos:   dotnet test --filter "Category=Integration"
///   Um só:   dotnet test --filter "FullyQualifiedName~ValidateRG"
///
/// Os testes são pulados automaticamente se o arquivo de configuração
/// ou os documentos de teste não forem encontrados.
/// </summary>
[Trait("Category", "Integration")]
public class DocumentValidationServiceIntegrationTests
{
    private readonly DocumentValidationService? _service;
    private readonly IConfiguration _config;
    private readonly string _testDocumentsPath;

    public DocumentValidationServiceIntegrationTests()
    {
        _testDocumentsPath = Path.Combine(
            Directory.GetCurrentDirectory(), "TestDocuments");

        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.IntegrationTests.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var endpoint = _config["AzureDocumentIntelligence:Endpoint"];
        var apiKey = _config["AzureDocumentIntelligence:ApiKey"];

        if (!string.IsNullOrWhiteSpace(endpoint) && !string.IsNullOrWhiteSpace(apiKey))
        {
            var client = new DocumentIntelligenceClient(
                new Uri(endpoint),
                new AzureKeyCredential(apiKey));
            _service = new DocumentValidationService(client);
        }
    }

    // -------------------------------------------------------------------------
    // RG
    // -------------------------------------------------------------------------

    [SkippableFact]
    public async Task ValidateRG_ComDadosCorretos_DeveRetornarTrue()
    {
        Skip.If(_service is null, "Configure appsettings.IntegrationTests.json para rodar este teste.");
        var docPath = GetDocumentPath("RG");
        Skip.If(!File.Exists(docPath), $"Arquivo de teste não encontrado: {docPath}");

        // Preencha os dados que EXISTEM no documento de teste
        var candidato = CriarCandidatoParaTesteRG(
            identidadeNumero: "1234567",
            identidadeUF: "SP",
            identidadeDataEmissao: new DateTime(2015, 6, 20));

        await using var stream = File.OpenRead(docPath);
        var resultado = await _service!.ValidateRGAsync(candidato, stream);

        resultado.Should().BeTrue("os dados preenchidos devem corresponder ao documento");
    }

    [SkippableFact]
    public async Task ValidateRG_ComDadosIncorretos_DeveRetornarFalse()
    {
        Skip.If(_service is null, "Configure appsettings.IntegrationTests.json para rodar este teste.");
        var docPath = GetDocumentPath("RG");
        Skip.If(!File.Exists(docPath), $"Arquivo de teste não encontrado: {docPath}");

        var candidato = CriarCandidatoParaTesteRG(
            identidadeNumero: "0000000",   // número errado
            identidadeUF: "XX",
            identidadeDataEmissao: new DateTime(2000, 1, 1));

        await using var stream = File.OpenRead(docPath);
        var resultado = await _service!.ValidateRGAsync(candidato, stream);

        resultado.Should().BeFalse("dados incorretos não devem passar na validação");
    }

    // -------------------------------------------------------------------------
    // CNH
    // -------------------------------------------------------------------------

    [SkippableFact]
    public async Task ValidateCNH_ComDadosCorretos_DeveRetornarTrue()
    {
        Skip.If(_service is null, "Configure appsettings.IntegrationTests.json para rodar este teste.");
        var docPath = GetDocumentPath("CNH");
        Skip.If(!File.Exists(docPath), $"Arquivo de teste não encontrado: {docPath}");

        // Preencha os dados que EXISTEM no documento de teste
        var candidato = CriarCandidatoParaTesteCNH(
            cnhNumero: "12345678900",
            cpf: "12345678900",
            dataNascimento: new DateTime(1990, 5, 15),
            cnhDataVencimento: new DateTime(2028, 5, 15));

        await using var stream = File.OpenRead(docPath);
        var resultado = await _service!.ValidateCNHAsync(candidato, stream);

        resultado.Should().BeTrue("os dados preenchidos devem corresponder ao documento");
    }

    [SkippableFact]
    public async Task ValidateCNH_ComDadosIncorretos_DeveRetornarFalse()
    {
        Skip.If(_service is null, "Configure appsettings.IntegrationTests.json para rodar este teste.");
        var docPath = GetDocumentPath("CNH");
        Skip.If(!File.Exists(docPath), $"Arquivo de teste não encontrado: {docPath}");

        var candidato = CriarCandidatoParaTesteCNH(
            cnhNumero: "00000000000",   // número errado
            cpf: "00000000000",
            dataNascimento: new DateTime(1900, 1, 1),
            cnhDataVencimento: new DateTime(2000, 1, 1));

        await using var stream = File.OpenRead(docPath);
        var resultado = await _service!.ValidateCNHAsync(candidato, stream);

        resultado.Should().BeFalse("dados incorretos não devem passar na validação");
    }

    // -------------------------------------------------------------------------
    // Comprovante de Residência
    // -------------------------------------------------------------------------

    [SkippableFact]
    public async Task ValidateComprovanteResidencia_ComDadosCorretos_DeveRetornarTrue()
    {
        Skip.If(_service is null, "Configure appsettings.IntegrationTests.json para rodar este teste.");
        var docPath = GetDocumentPath("ComprovanteResidencia");
        Skip.If(!File.Exists(docPath), $"Arquivo de teste não encontrado: {docPath}");

        // Preencha com os dados que EXISTEM no documento de teste
        var candidato = CriarCandidatoParaTesteComprovante(
            nome: "João da Silva",
            rua: "Rua das Flores",
            cidade: "São Paulo",
            cep: "01310100");

        await using var stream = File.OpenRead(docPath);
        var resultado = await _service!.ValidateComprovanteResidenciaAsync(candidato, stream);

        resultado.Should().BeTrue("os dados preenchidos devem corresponder ao documento");
    }

    [SkippableFact]
    public async Task ValidateComprovanteResidencia_ComDadosIncorretos_DeveRetornarFalse()
    {
        Skip.If(_service is null, "Configure appsettings.IntegrationTests.json para rodar este teste.");
        var docPath = GetDocumentPath("ComprovanteResidencia");
        Skip.If(!File.Exists(docPath), $"Arquivo de teste não encontrado: {docPath}");

        var candidato = CriarCandidatoParaTesteComprovante(
            nome: "Nome Inexistente",
            rua: "Rua Inexistente",
            cidade: "Cidade Inexistente",
            cep: "00000000");

        await using var stream = File.OpenRead(docPath);
        var resultado = await _service!.ValidateComprovanteResidenciaAsync(candidato, stream);

        resultado.Should().BeFalse("dados incorretos não devem passar na validação");
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private string GetDocumentPath(string documentKey)
    {
        var configPath = _config[$"DocumentosParaTeste:{documentKey}"];
        if (!string.IsNullOrWhiteSpace(configPath))
            return Path.IsPathRooted(configPath) ? configPath : Path.Combine(Directory.GetCurrentDirectory(), configPath);

        // Fallback para convenção de nomes
        var defaults = new Dictionary<string, string>
        {
            ["RG"] = "rg_teste.jpg",
            ["CNH"] = "cnh_teste.jpg",
            ["ComprovanteResidencia"] = "comprovante_teste.pdf"
        };
        return Path.Combine(_testDocumentsPath, defaults[documentKey]);
    }

    private static global::PeopleIO.Domain.Entity.Candidato CriarCandidatoParaTesteRG(
        string identidadeNumero, string identidadeUF, DateTime identidadeDataEmissao)
    {
        return new global::PeopleIO.Domain.Entity.Candidato
        {
            Nome = "Teste",
            CPF = "12345678900",
            DataNascimento = DateTime.Now.AddYears(-30),
            Email = "teste@teste.com",
            Telefone = "11999999999",
            Endereco = new Endereco { Rua = "Rua Teste", Cidade = "SP", CEP = "01310100" },
            IdentidadeNumero = identidadeNumero,
            IdentidadeUF = identidadeUF,
            IdentidadeDataEmissao = identidadeDataEmissao,
            CTPSNumero = "0",
            CTPSSerie = "0",
            CTPSDataEmissao = DateTime.Now,
            CTPSUF = "SP",
            CorRaca = CorRaca.Branca,
            Sexo = Sexo.Masculino,
            Escolaridade = "Superior",
            EstadoCivil = "Solteiro",
            Naturalidade = "SP",
            Nacionalidade = "Brasileiro",
            Experiencias = []
        };
    }

    private static global::PeopleIO.Domain.Entity.Candidato CriarCandidatoParaTesteCNH(
        string cnhNumero, string cpf, DateTime dataNascimento, DateTime cnhDataVencimento)
    {
        return new global::PeopleIO.Domain.Entity.Candidato
        {
            Nome = "Teste",
            CPF = cpf,
            DataNascimento = dataNascimento,
            Email = "teste@teste.com",
            Telefone = "11999999999",
            Endereco = new Endereco { Rua = "Rua Teste", Cidade = "SP", CEP = "01310100" },
            CNHNumero = cnhNumero,
            CNHDataVencimento = cnhDataVencimento,
            CTPSNumero = "0",
            CTPSSerie = "0",
            CTPSDataEmissao = DateTime.Now,
            CTPSUF = "SP",
            CorRaca = CorRaca.Branca,
            Sexo = Sexo.Masculino,
            Escolaridade = "Superior",
            EstadoCivil = "Solteiro",
            Naturalidade = "SP",
            Nacionalidade = "Brasileiro",
            Experiencias = []
        };
    }

    private static global::PeopleIO.Domain.Entity.Candidato CriarCandidatoParaTesteComprovante(
        string nome, string rua, string cidade, string cep)
    {
        return new global::PeopleIO.Domain.Entity.Candidato
        {
            Nome = nome,
            CPF = "12345678900",
            DataNascimento = DateTime.Now.AddYears(-30),
            Email = "teste@teste.com",
            Telefone = "11999999999",
            Endereco = new Endereco { Rua = rua, Cidade = cidade, CEP = cep },
            CTPSNumero = "0",
            CTPSSerie = "0",
            CTPSDataEmissao = DateTime.Now,
            CTPSUF = "SP",
            CorRaca = CorRaca.Branca,
            Sexo = Sexo.Masculino,
            Escolaridade = "Superior",
            EstadoCivil = "Solteiro",
            Naturalidade = "SP",
            Nacionalidade = "Brasileiro",
            Experiencias = []
        };
    }
}