using Mapster;
using PeopleIO.Application.Results;
using PeopleIO.Communication;
using PeopleIO.Domain.Contract;
using PeopleIO.Application.Services;

namespace PeopleIO.Application.Services.Candidato.Register;

public class RegisterCandidatoService : IRegisterCandidatoService
{
    private readonly ICandidatoRepository _candidatoRepository;
    private readonly ICRUDRepository<Domain.Entity.Candidato> _crudRepository;
    private readonly IDocumentValidationService _documentValidationService;
    private readonly IBlobStorageService _blobStorageService;

    public RegisterCandidatoService(
        ICandidatoRepository candidatoRepository, 
        ICRUDRepository<Domain.Entity.Candidato> crudRepository,
        IDocumentValidationService documentValidationService,
        IBlobStorageService blobStorageService)
    {
        _candidatoRepository = candidatoRepository;
        _crudRepository = crudRepository;
        _documentValidationService = documentValidationService;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<CandidatoResponse>> ExecuteAsync(RequestRegisterCandidato request, CancellationToken ct)
    {
        Validate(request);
        
        if(await _candidatoRepository.GetByCPFAsync(request.CPF))
            return Result<CandidatoResponse>.Failure("O CPF informado já existe no sistema.");
        
        var candidato = request.Adapt<Domain.Entity.Candidato>();

        if (request.ArquivoRG?.Url is not null)
        {
            var blobName = GetBlobNameFromUrl(request.ArquivoRG.Url);
            using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);
            var isValid = await _documentValidationService.ValidateRGAsync(candidato, stream);
            if (!isValid)
                return Result<CandidatoResponse>.Failure("Os dados do RG não correspondem ao documento enviado.");
        }

        if (request.ArquivoCNH?.Url is not null)
        {
            var blobName = GetBlobNameFromUrl(request.ArquivoCNH.Url);
            using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);
            var isValid = await _documentValidationService.ValidateCNHAsync(candidato, stream);
            if (!isValid)
                return Result<CandidatoResponse>.Failure("Os dados da CNH não correspondem ao documento enviado.");
        }

        if (request.ArquivoComprovanteResidencia?.Url is not null)
        {
            var blobName = GetBlobNameFromUrl(request.ArquivoComprovanteResidencia.Url);
            using var stream = await _blobStorageService.GetBlobStreamAsync(blobName);
            var isValid = await _documentValidationService.ValidateComprovanteResidenciaAsync(candidato, stream);
            if (!isValid)
                return Result<CandidatoResponse>.Failure("Os dados do comprovante de residência não correspondem ao documento enviado.");
        }
        
        await _crudRepository.AddAsync(candidato, ct);
        
        var response = new CandidatoResponse(
            candidato.Id, 
            candidato.Nome, 
            candidato.Email);
        
        return Result<CandidatoResponse>.Success(response);
    }

    private void Validate(RequestRegisterCandidato request)
    {
        var validate = new RegisterCandidatoValidator();
        var result = validate.Validate(request);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ApplicationException(string.Join(Environment.NewLine, errorMessages));
        }
    }

    private string GetBlobNameFromUrl(string url)
    {
        return new Uri(url).Segments.Last();
    }
}