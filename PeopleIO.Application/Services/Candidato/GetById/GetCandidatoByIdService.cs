using PeopleIO.Application.Services.Candidato.GetAll;
using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Candidato.GetById;

public class GetCandidatoByIdService : IGetCandidatoByIdService
{
    private readonly ICandidatoRepository _repository;
    public GetCandidatoByIdService(ICandidatoRepository repository)
    {
        _repository =  repository;
    }

    public async Task<Domain.Entity.Candidato> Execute(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID inválido.");

        var colaborador = await _repository.GetByIdAsync(id);
        if (colaborador is null)
            throw new KeyNotFoundException($"Colaborador com ID {id} não encontrado.");

        return colaborador;

    }
}