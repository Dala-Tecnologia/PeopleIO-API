using PeopleIO.Application.Results;
using PeopleIO.Application.Services.Candidato.GetAll;
using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Candidato.GetAll;

public class GetAllCandidatosService : IGetAllCandidatosService
{
    private readonly ICRUDRepository<Domain.Entity.Candidato> _ctx;

    public GetAllCandidatosService(ICRUDRepository<Domain.Entity.Candidato> ctx)
    {
        _ctx = ctx;
    }

    public async Task<Result<IEnumerable<Domain.Entity.Candidato>>> Execute(CancellationToken ct)
    {
        var colaboradores = await _ctx.GetAllAsync(ct, c => c.Experiencias!);
        return Result<IEnumerable<Domain.Entity.Candidato>>.Success(colaboradores);
    }
}