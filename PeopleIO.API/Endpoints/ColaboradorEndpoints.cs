using PeopleIO.Application.Services.Colaborador.Delete;
using PeopleIO.Application.Services.Colaborador.GetAll;
using PeopleIO.Application.Services.Colaborador.GetById;
using PeopleIO.Application.Services.Colaborador.Register;
using PeopleIO.Communication;
using PeopleIO.Domain.Entity;

namespace PeapleIO.API.Endpoints;

public static class ColaboradorEndpoints
{
    public static void MapColaboradorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/colaboradores")
            .WithTags("Colaboradores")
            .RequireAuthorization();

        group.MapGet("", (IGetAllColaboradoresService service) => Results.Ok(service.Execute()));
        group.MapGet("/{id:guid}", async (Guid id, IGetColaboradorByIdService service) =>
        {
            var colaborador = await service.Execute(id);
            return colaborador is null
                ? Results.NotFound()
                : Results.Ok(colaborador);
        });
        group.MapPost("", async (RequestRegisterColaborador request, IRegisterColaboradorService service) =>
        {
            var result = await service.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Results.Created($"/api/v1/colaboradores/{result.Value!.Id}", result.Value);
            }
            return Results.BadRequest(new { Errors = new[] { result.Error } });
        })
        .WithName("CreateColaborador");
        group.MapDelete("/{id:guid}", async (Guid id, IRemoveColaboradorService service) =>
        {
            try
            {
                var success = await service.Execute(id);
                return success ? Results.NoContent() : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem("Erro ao remover colaborador: " + ex.Message);
            }
        })
        .WithName("DeleteColaborador");

    }
}