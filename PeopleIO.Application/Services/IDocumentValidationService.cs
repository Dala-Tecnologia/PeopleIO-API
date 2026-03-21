namespace PeopleIO.Application.Services;

public interface IDocumentValidationService
{
    Task<bool> ValidateRGAsync(global::PeopleIO.Domain.Entity.Candidato candidato, Stream arquivoStream);
    Task<bool> ValidateRGAsync(string identidadeNumero, string identidadeUF, DateTime? dataEmissao, Stream arquivoStream);

    Task<bool> ValidateCNHAsync(global::PeopleIO.Domain.Entity.Candidato candidato, Stream arquivoStream);
    Task<bool> ValidateCNHAsync(string cnhNumero, string cpf, DateTime dataNascimento, DateTime? dataVencimento, Stream arquivoStream);

    Task<bool> ValidateComprovanteResidenciaAsync(global::PeopleIO.Domain.Entity.Candidato candidato, Stream arquivoStream);
    Task<bool> ValidateComprovanteResidenciaAsync(string nome, string rua, string cidade, string cep, Stream arquivoStream);
}
