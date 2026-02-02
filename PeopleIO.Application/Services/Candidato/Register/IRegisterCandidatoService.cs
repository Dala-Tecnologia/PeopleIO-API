using PeopleIO.Application.Results;
using PeopleIO.Communication;

namespace PeopleIO.Application.Services.Candidato.Register;

public interface IRegisterCandidatoService
{
    Task<Result<CandidatoResponse>> ExecuteAsync(RequestRegisterCandidato request);
}