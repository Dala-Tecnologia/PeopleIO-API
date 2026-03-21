using PeopleIO.Application.Results;
using PeopleIO.Communication;

namespace PeopleIO.Application.Services.Documento.ValidarComprovante;

public interface IValidarComprovanteService
{
    Task<Result<DocumentoValidacaoResponse>> ExecuteAsync(RequestValidarComprovanteResidencia request, CancellationToken ct);
}