using PeopleIO.Application.Services.Candidato.GetById;
using PeopleIO.Application.Services.Candidato.Delete;
using PeopleIO.Application.Services.Candidato.GetAll;
using PeopleIO.Application.Services.Candidato.Register;
using PeopleIO.Communication;

namespace PeapleIO.API.Endpoints;

public static class CandidatoEndpoints
{
    public static void MapCandidatoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/candidato")
            .WithTags("Candidato")
            .RequireAuthorization();

        group.MapGet("", (IGetAllCandidatosService service) => Results.Ok(service.Execute()));
        group.MapGet("/{id:guid}", async (Guid id, IGetCandidatoByIdService service) =>
        {
            var candidato = await service.Execute(id);
            return candidato is null
                ? Results.NotFound()
                : Results.Ok(candidato);
        });
        group.MapPost("", async (RequestRegisterCandidato request, IRegisterCandidatoService service) =>
        {
            var result = await service.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/v1/candidato/{result.Value!.Id}", result.Value);
            }
            return Results.BadRequest(new { Errors = new[] { result.Error } });
        })
        .WithName("CreateCandidato");
        group.MapDelete("/{id:guid}", async (Guid id, IRemoveCandidatoService service) =>
        {
            try
            {
                var success = await service.Execute(id);
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