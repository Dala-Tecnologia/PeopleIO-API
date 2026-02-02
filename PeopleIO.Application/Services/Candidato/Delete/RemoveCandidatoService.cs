using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Candidato.Delete;

public class RemoveCandidatoService : IRemoveCandidatoService
{
    private readonly ICandidatoRepository _candidatoRepository;
    public RemoveCandidatoService(ICandidatoRepository candidatoRepository)
    {
        _candidatoRepository = candidatoRepository;
    }

    public async Task<bool> Execute(Guid id)
    {
        var colaborador = await _candidatoRepository.GetByIdAsync(id);
        if (colaborador is null)
            return false;

        await _candidatoRepository.DeleteAsync(id);
        return true;

    }
}