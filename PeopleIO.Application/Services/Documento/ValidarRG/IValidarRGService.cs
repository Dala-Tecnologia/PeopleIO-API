using PeopleIO.Application.Results;
using PeopleIO.Communication;

namespace PeopleIO.Application.Services.Documento.ValidarRG;

public interface IValidarRGService
{
    Task<Result<DocumentoValidacaoResponse>> ExecuteAsync(RequestValidarRG request, CancellationToken ct);
}