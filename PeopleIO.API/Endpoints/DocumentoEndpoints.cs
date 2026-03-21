using PeopleIO.Application.Services.Documento.ValidarCNH;
using PeopleIO.Application.Services.Documento.ValidarComprovante;
using PeopleIO.Application.Services.Documento.ValidarRG;
using PeopleIO.Communication;

namespace PeopleIO.API.Endpoints;

public static class DocumentoEndpoints
{
    public static void MapDocumentoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/documento")
            .WithTags("Documento")
            .RequireAuthorization();

        group.MapPost("/validar/rg", async (RequestValidarRG request, IValidarRGService service, CancellationToken ct) =>
        {
            var result = await service.ExecuteAsync(request, ct);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.UnprocessableEntity(new { Errors = new[] { result.Error } });
        })
        .WithName("ValidarRG");

        group.MapPost("/validar/cnh", async (RequestValidarCNH request, IValidarCNHService service, CancellationToken ct) =>
        {
            var result = await service.ExecuteAsync(request, ct);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.UnprocessableEntity(new { Errors = new[] { result.Error } });
        })
        .WithName("ValidarCNH");

        group.MapPost("/validar/comprovante-residencia", async (RequestValidarComprovanteResidencia request, IValidarComprovanteService service, CancellationToken ct) =>
        {
            var result = await service.ExecuteAsync(request, ct);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.UnprocessableEntity(new { Errors = new[] { result.Error } });
        })
        .WithName("ValidarComprovanteResidencia");
    }
}