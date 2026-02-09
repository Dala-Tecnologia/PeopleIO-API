namespace PeopleIO.Application.Services.Candidato.GetById;

public interface IGetCandidatoByIdService
{
    Task<Domain.Entity.Candidato> Execute(Guid id, CancellationToken ct);
}