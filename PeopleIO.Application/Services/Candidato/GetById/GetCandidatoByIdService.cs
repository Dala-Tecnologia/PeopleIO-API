using PeopleIO.Application.Services.Candidato.GetAll;
using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Candidato.GetById;

public class GetCandidatoByIdService : IGetCandidatoByIdService
{
    private readonly ICRUDRepository<Domain.Entity.Candidato> _repository;
    public GetCandidatoByIdService(ICRUDRepository<Domain.Entity.Candidato> repository)
    {
        _repository =  repository;
    }

    public async Task<Domain.Entity.Candidato> Execute(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID inválido.");

        var colaborador = await _repository.GetByIdAsync(id, ct, c => c.Experiencias!);
        if (colaborador is null)
            throw new KeyNotFoundException($"Colaborador com ID {id} não encontrado.");

        return colaborador;

    }
}