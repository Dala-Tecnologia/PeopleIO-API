using Azure.Identity;
using PeopleIO.Application.Services.Candidato.GetById;
using PeopleIO.Application.Services.Candidato.Delete;
using PeopleIO.Application.Services.Candidato.GetAll;
using PeopleIO.Application.Services.Candidato.Register;
using PeopleIO.Application.Services.Candidato.Update;
using PeopleIO.Communication;

namespace PeapleIO.API.Endpoints;

public static class CandidatoEndpoints
{
    public static void MapCandidatoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/candidato")
            .WithTags("Candidato")
            .RequireAuthorization();

        group.MapGet("", async (IGetAllCandidatosService service, CancellationToken ct) => 
            Results.Ok(await service.Execute(ct)));

        group.MapGet("/{id:guid}", async (Guid id, IGetCandidatoByIdService service, CancellationToken ct) =>
        {
            var candidato = await service.Execute(id, ct);
            return candidato is null
                ? Results.NotFound()
                : Results.Ok(candidato);
        });

        group.MapPost("", async (RequestRegisterCandidato request, IRegisterCandidatoService service, CancellationToken ct) =>
        {
            var result = await service.ExecuteAsync(request, ct);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/v1/candidato/{result.Value!.Id}", result.Value);
            }
            return Results.BadRequest(new { Errors = new[] { result.Error } });
        })
        .WithName("CreateCandidato");
        
        group.MapPut("/{id:guid}", async (Guid id, CandidatoDTO request, IUpdateCandidatoService service, HttpContext context, CancellationToken ct) => 
        {
            var result = await service.ExecuteAsync(id, request, context.User.Identity?.Name ?? "System", ct);
            if (result.Result is Microsoft.AspNetCore.Http.HttpResults.NotFound)
            {
                return Results.NotFound();
            }
            return Results.Ok();
        })
        .WithName("UpdateCandidato");
        
        group.MapDelete("/{id:guid}", async (Guid id, IRemoveCandidatoService service, CancellationToken ct) =>
        {
            try
            {
                var success = await service.Execute(id, ct);
                return success ? Results.NoContent() : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem("Erro ao remover candidato: " + ex.Message);
            }
        })
        .WithName("DeleteCandidato");
    }
}