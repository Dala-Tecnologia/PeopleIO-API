using PeopleIO.Application.Results;
using PeopleIO.Application.Services.Candidato.GetAll;
using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Candidato.GetAll;

public class GetAllCandidatosService : IGetAllCandidatosService
{
    private readonly ICandidatoRepository  _candidatoRepository;

    public GetAllCandidatosService(ICandidatoRepository candidatoRepository)
    {
        _candidatoRepository = candidatoRepository;
    }

    public Result<IEnumerable<Domain.Entity.Candidato>> Execute()
    {
        var colaboradores = _candidatoRepository.GetAll();
        return Result<IEnumerable<Domain.Entity.Candidato>>.Success(colaboradores);
    }
}