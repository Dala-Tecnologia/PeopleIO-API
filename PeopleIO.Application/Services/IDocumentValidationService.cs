using PeopleIO.Domain.Entity;

namespace PeopleIO.Application.Services;

public interface IDocumentValidationService
{
    Task<bool> ValidateRGAsync(Candidato candidato, Stream arquivoStream);
    Task<bool> ValidateCNHAsync(Candidato candidato, Stream arquivoStream);
    Task<bool> ValidateComprovanteResidenciaAsync(Candidato candidato, Stream arquivoStream);
}
