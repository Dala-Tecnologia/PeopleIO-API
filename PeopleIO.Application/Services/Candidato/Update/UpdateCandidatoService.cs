using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PeopleIO.Communication;
using PeopleIO.Domain.Contract;
using PeopleIO.Application.Services;

namespace PeopleIO.Application.Services.Candidato.Update;

public class UpdateCandidatoService : IUpdateCandidatoService
{
    private readonly ICRUDRepository<Domain.Entity.Candidato> _repo;
    private readonly ILogger<UpdateCandidatoService> _logger;
    private readonly IDocumentValidationService _documentValidationService;
    private readonly IBlobStorageService _blobStorageService;

    public UpdateCandidatoService(
        ICRUDRepository<Domain.Entity.Candidato> repo, 
        ILogger<UpdateCandidatoService> logger,
        IDocumentValidationService documentValidationService,
        IBlobStorageService blobStorageService)
    {
        _repo = repo;
        _logger = logger;
        _documentValidationService = documentValidationService;
        _blobStorageService = blobStorageService;
    }

    public async Task<Results<Ok, NotFound, BadRequest<string>>> ExecuteAsync(Guid id, CandidatoDTO request, 
        string userName, CancellationToken ct)
    {
        var candidato = request.ToEntity();

        if (request.ArquivoRG?.Url is not null)
        {
            var blobName = GetBlobNameFromUrl(request.ArquivoRG.Url);
            using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);
            var isValid = await _documentValidationService.ValidateRGAsync(candidato, stream);
            if (!isValid)
                return TypedResults.BadRequest("Os dados do RG não correspondem ao documento enviado.");
        }

        if (request.ArquivoCNH?.Url is not null)
        {
            var blobName = GetBlobNameFromUrl(request.ArquivoCNH.Url);
            using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);
            var isValid = await _documentValidationService.ValidateCNHAsync(candidato, stream);
            if (!isValid)
                return TypedResults.BadRequest("Os dados da CNH não correspondem ao documento enviado.");
        }

        if (request.ArquivoComprovanteResidencia?.Url is not null)
        {
            var blobName = GetBlobNameFromUrl(request.ArquivoComprovanteResidencia.Url);
            using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);
            var isValid = await _documentValidationService.ValidateComprovanteResidenciaAsync(candidato, stream);
            if (!isValid)
                return TypedResults.BadRequest("Os dados do comprovante de residência não correspondem ao documento enviado.");
        }

        var result = await _repo.UpdateAsync(id, (Domain.Entity.Candidato entity) =>
        {
            entity.UpdateFrom(request);
            entity.SetUpdated(userName);
        }, ct);

        return result > 0 ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private string GetBlobNameFromUrl(string url)
    {
        return new Uri(url).Segments.Last();
    }
}