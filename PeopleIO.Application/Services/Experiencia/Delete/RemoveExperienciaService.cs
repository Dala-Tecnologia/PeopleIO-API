using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Experiencia.Delete;

public class RemoveExperienciaService : IRemoveExperienciaService
{
    private readonly ICRUDRepository<Domain.Entity.Experiencia> _ctx;
    public RemoveExperienciaService(ICRUDRepository<Domain.Entity.Experiencia> ctx)
    {
        _ctx = ctx;
    }

    public async Task<bool> Execute(Guid id, CancellationToken ct)
    {
        var experiencia = await _ctx.GetByIdAsync(id, ct);
        if (experiencia is null)
            return false;

        await _ctx.DeleteAsync(id, ct);
        return true;

    }
}