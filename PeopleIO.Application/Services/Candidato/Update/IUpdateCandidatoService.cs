using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PeopleIO.Communication;

namespace PeopleIO.Application.Services.Candidato.Update;

public interface IUpdateCandidatoService
{
    Task<Results<Ok, NotFound, BadRequest<string>>> ExecuteAsync(Guid id, CandidatoDTO request,
        string userName, CancellationToken ct);
}