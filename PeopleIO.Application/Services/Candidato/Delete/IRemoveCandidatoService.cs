namespace PeopleIO.Application.Services.Candidato.Delete;

public interface IRemoveCandidatoService
{
    Task<bool> Execute(Guid id, CancellationToken ct);
}