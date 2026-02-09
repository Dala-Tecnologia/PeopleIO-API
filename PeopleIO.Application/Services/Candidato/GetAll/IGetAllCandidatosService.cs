using PeopleIO.Application.Results;

namespace PeopleIO.Application.Services.Candidato.GetAll;

public interface IGetAllCandidatosService
{
    Task<Result<IEnumerable<Domain.Entity.Candidato>>> Execute(CancellationToken ct);
}