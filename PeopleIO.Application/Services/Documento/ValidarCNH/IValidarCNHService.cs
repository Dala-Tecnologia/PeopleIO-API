using PeopleIO.Application.Results;
using PeopleIO.Communication;

namespace PeopleIO.Application.Services.Documento.ValidarCNH;

public interface IValidarCNHService
{
    Task<Result<DocumentoValidacaoResponse>> ExecuteAsync(RequestValidarCNH request, CancellationToken ct);
}