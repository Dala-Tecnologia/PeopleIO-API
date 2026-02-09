using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Candidato.Delete;

public class RemoveCandidatoService : IRemoveCandidatoService
{
    private readonly ICRUDRepository<Domain.Entity.Candidato> _ctx;
    public RemoveCandidatoService(ICRUDRepository<Domain.Entity.Candidato> ctx)
    {
        _ctx = ctx;
    }

    public async Task<bool> Execute(Guid id, CancellationToken ct)
    {
        var colaborador = await _ctx.GetByIdAsync(id, ct);
        if (colaborador is null)
            return false;

        await _ctx.DeleteAsync(id, ct);
        return true;

    }
}