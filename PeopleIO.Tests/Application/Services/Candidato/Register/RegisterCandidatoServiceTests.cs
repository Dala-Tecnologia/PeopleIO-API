using FluentAssertions;
using Mapster;
using Moq;
using PeopleIO.Application.Results;
using PeopleIO.Application.Services;
using PeopleIO.Application.Services.Candidato.Register;
using PeopleIO.Communication;
using PeopleIO.Domain.Contract;
using PeopleIO.Domain.Entity;
using System.Linq.Expressions;
using Xunit;

namespace PeopleIO.Tests.Application.Services.Candidato.Register;

public class RegisterCandidatoServiceTests
{
    private readonly Mock<ICandidatoRepository> _candidatoRepositoryMock;
    private readonly Mock<ICRUDRepository<Domain.Entity.Candidato>> _crudRepositoryMock;
    private readonly Mock<IDocumentValidationService> _documentValidationServiceMock;
    private readonly Mock<IBlobStorageService> _blobStorageServiceMock;
    private readonly RegisterCandidatoService _service;

    public RegisterCandidatoServiceTests()
    {
        _candidatoRepositoryMock = new Mock<ICandidatoRepository>();
        _crudRepositoryMock = new Mock<ICRUDRepository<Domain.Entity.Candidato>>();
        _documentValidationServiceMock = new Mock<IDocumentValidationService>();
        _blobStorageServiceMock = new Mock<IBlobStorageService>();

        _service = new RegisterCandidatoService(
            _candidatoRepositoryMock.Object,
            _crudRepositoryMock.Object,
            _documentValidationServiceMock.Object,
            _blobStorageServiceMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Failure_When_RG_Validation_Fails()
    {
        // Arrange
        var request = CreateValidRequest();
        request = request with { ArquivoRG = new DocumentoDTO("rg.pdf", "http://blob/rg.pdf", DateTime.Now, "application/pdf") };

        _candidatoRepositoryMock.Setup(x => x.GetByCPFAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _blobStorageServiceMock.Setup(x => x.GetBlobStreamAsync(It.IsAny<string>()))
            .ReturnsAsync(new MemoryStream());

        _documentValidationServiceMock.Setup(x => x.ValidateRGAsync(It.IsAny<Domain.Entity.Candidato>(), It.IsAny<Stream>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> act = async () => await _service.ExecuteAsync(request, CancellationToken.None);

        // Assert
        // Como o serviço lança ApplicationException na validação do validator, preciso garantir que o request seja válido primeiro.
        // Mas a validação do documento retorna Result.Failure.
        // O Validate(request) roda antes.

        // Se Validate(request) passar, então verificamos o Result.
        var result = await _service.ExecuteAsync(request, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Os dados do RG não correspondem ao documento enviado.");
    }
    
    private RequestRegisterCandidato CreateValidRequest()
    {
        return new RequestRegisterCandidato(
            "Nome Teste",
            null,
            "12345678900",
            DateTime.Now.AddYears(-20),
            "test@test.com",
            "11999999999",
            new EnderecoDTO("Rua Teste", "123", "Bairro Teste", "Cidade Teste", "SP", "12345678"),
            "Desenvolvedor", 
            "TI", 
            DateTime.Now, 
            "12.345.678-9", 
            "SSP", 
            "SP", 
            DateTime.Now.AddYears(-5), 
            "123456", 
            "001", 
            DateTime.Now.AddYears(-2), 
            "SP", 
            null, null, null, null, 
            null, null, null, null, null, 
            "Branca", 
            "Masculino", 
            "Superior Completo", 
            "Solteiro", 
            "São Paulo", 
            "Brasileira", 
            new List<ExperienciaDTO>(), 
            null, null, null, null, null, null
        );
    }
}
