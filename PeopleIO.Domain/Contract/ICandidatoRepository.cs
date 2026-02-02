using PeopleIO.Domain.Entity;

namespace PeopleIO.Domain.Contract;

public interface ICandidatoRepository
{
    Task<int> RegisterAsync(Candidato candidato);
    Task<Candidato?> GetByIdAsync(Guid id);
    Task<bool> GetByCPFAsync(string cpf);
    IEnumerable<Candidato> GetAll();
    Task Update(Candidato candidato);
    Task DeleteAsync(Guid id);

}