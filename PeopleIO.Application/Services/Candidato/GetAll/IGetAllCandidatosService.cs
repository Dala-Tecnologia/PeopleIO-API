using PeopleIO.Application.Results;

namespace PeopleIO.Application.Services.Candidato.GetAll;

public interface IGetAllCandidatosService
{
    Result<IEnumerable<Domain.Entity.Candidato>> Execute();
}