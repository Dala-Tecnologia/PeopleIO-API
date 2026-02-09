using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PeopleIO.Communication;
using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Candidato.Update;

public class UpdateCandidatoService : IUpdateCandidatoService
{
    private readonly ICRUDRepository<Domain.Entity.Candidato> _repo;
    private readonly ILogger<UpdateCandidatoService> _logger;

    public UpdateCandidatoService(ICRUDRepository<Domain.Entity.Candidato> repo, 
        ILogger<UpdateCandidatoService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Results<Ok, NotFound>> ExecuteAsync(Guid id, CandidatoDTO request, 
        string userName, CancellationToken ct)
    {
        var result = await _repo.UpdateAsync(id, (Domain.Entity.Candidato entity) =>
        {
            entity.UpdateFrom(request);
            entity.SetUpdated(userName);
        }, ct);

        return result > 0 ? TypedResults.Ok() : TypedResults.NotFound();

    }
}