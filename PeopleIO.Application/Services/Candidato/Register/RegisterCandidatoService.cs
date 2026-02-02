using Mapster;
using PeopleIO.Application.Results;
using PeopleIO.Communication;
using PeopleIO.Domain.Contract;

namespace PeopleIO.Application.Services.Candidato.Register;

public class RegisterCandidatoService : IRegisterCandidatoService
{
    private readonly ICandidatoRepository _candidatoRepository;

    public RegisterCandidatoService(ICandidatoRepository candidatoRepository)
    {
        _candidatoRepository = candidatoRepository;
    }

    public async Task<Result<CandidatoResponse>> ExecuteAsync(RequestRegisterCandidato request)
    {
        Validate(request);
        
        if(await _candidatoRepository.GetByCPFAsync(request.CPF))
            return Result<CandidatoResponse>.Failure("O CPF informado já existe no sistema.");
        
        var colaborador = request.Adapt<Domain.Entity.Candidato>();
        
        await _candidatoRepository.RegisterAsync(colaborador);
        
        var response = new CandidatoResponse(
            colaborador.Id, 
            colaborador.Nome, 
            colaborador.Email);
        
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
}