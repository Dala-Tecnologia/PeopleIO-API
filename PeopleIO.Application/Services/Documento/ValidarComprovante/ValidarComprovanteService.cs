using PeopleIO.Application.Results;
using PeopleIO.Communication;

namespace PeopleIO.Application.Services.Documento.ValidarComprovante;

public class ValidarComprovanteService : IValidarComprovanteService
{
    private readonly IDocumentValidationService _documentValidationService;
    private readonly IBlobStorageService _blobStorageService;

    public ValidarComprovanteService(IDocumentValidationService documentValidationService, IBlobStorageService blobStorageService)
    {
        _documentValidationService = documentValidationService;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<DocumentoValidacaoResponse>> ExecuteAsync(RequestValidarComprovanteResidencia request, CancellationToken ct)
    {
        var blobName = string.Concat(new Uri(request.BlobUrl).Segments.Skip(2));
        using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);

        var isValid = await _documentValidationService.ValidateComprovanteResidenciaAsync(
            request.Nome, request.Rua, request.Cidade, request.CEP, stream);

        return isValid
            ? Result<DocumentoValidacaoResponse>.Success(new DocumentoValidacaoResponse(true, null))
            : Result<DocumentoValidacaoResponse>.Failure("Os dados do comprovante de residência não correspondem ao documento enviado.");
    }
}