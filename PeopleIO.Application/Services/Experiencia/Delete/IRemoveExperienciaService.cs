namespace PeopleIO.Application.Services.Experiencia.Delete;

public interface IRemoveExperienciaService
{
    Task<bool> Execute(Guid id, CancellationToken ct);
}