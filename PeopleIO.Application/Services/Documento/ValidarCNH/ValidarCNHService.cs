using PeopleIO.Application.Results;
using PeopleIO.Communication;

namespace PeopleIO.Application.Services.Documento.ValidarCNH;

public class ValidarCNHService : IValidarCNHService
{
    private readonly IDocumentValidationService _documentValidationService;
    private readonly IBlobStorageService _blobStorageService;

    public ValidarCNHService(IDocumentValidationService documentValidationService, IBlobStorageService blobStorageService)
    {
        _documentValidationService = documentValidationService;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<DocumentoValidacaoResponse>> ExecuteAsync(RequestValidarCNH request, CancellationToken ct)
    {
        var blobName = string.Concat(new Uri(request.BlobUrl).Segments.Skip(2));
        using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);

        var isValid = await _documentValidationService.ValidateCNHAsync(
            request.CNHNumero, request.CPF, request.DataNascimento, request.CNHDataVencimento, stream);

        return isValid
            ? Result<DocumentoValidacaoResponse>.Success(new DocumentoValidacaoResponse(true, null))
            : Result<DocumentoValidacaoResponse>.Failure("Os dados da CNH não correspondem ao documento enviado.");
    }
}