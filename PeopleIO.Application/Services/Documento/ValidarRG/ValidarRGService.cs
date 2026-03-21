using PeopleIO.Application.Results;
using PeopleIO.Communication;

namespace PeopleIO.Application.Services.Documento.ValidarRG;

public class ValidarRGService : IValidarRGService
{
    private readonly IDocumentValidationService _documentValidationService;
    private readonly IBlobStorageService _blobStorageService;

    public ValidarRGService(IDocumentValidationService documentValidationService, IBlobStorageService blobStorageService)
    {
        _documentValidationService = documentValidationService;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<DocumentoValidacaoResponse>> ExecuteAsync(RequestValidarRG request, CancellationToken ct)
    {
        var blobName = string.Concat(new Uri(request.BlobUrl).Segments.Skip(2));
        using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);

        var isValid = await _documentValidationService.ValidateRGAsync(
            request.IdentidadeNumero, request.IdentidadeUF, request.IdentidadeDataEmissao, stream);

        return isValid
            ? Result<DocumentoValidacaoResponse>.Success(new DocumentoValidacaoResponse(true, null))
            : Result<DocumentoValidacaoResponse>.Failure("Os dados do RG não correspondem ao documento enviado.");
    }
}