using Microsoft.EntityFrameworkCore;
using PeopleIO.Domain.Contract;
using PeopleIO.Domain.Entity;
using PeopleIO.Infrastructure.Context;

namespace PeopleIO.Infrastructure.Repository;

public class CandidatoRepository : ICandidatoRepository
{
    private readonly PeopleIoContext _ctx;

    public CandidatoRepository(PeopleIoContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> RegisterAsync(Candidato candidato)
    {
        await _ctx.Candidato.AddAsync(candidato);
        return await _ctx.SaveChangesAsync();
    }

    public Task<Candidato?> GetByIdAsync(Guid id) =>
        _ctx.Candidato.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);    
    
    public Task<bool> GetByCPFAsync(string cpf) =>
        _ctx.Candidato.AsNoTracking().AnyAsync(c => c.CPF == cpf);
    
    public IEnumerable<Candidato> GetAll() =>
        _ctx.Candidato
            .ToList();
    
    public async Task Update(Candidato candidato)
    {
        _ctx.Candidato.Update(candidato);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var candidato = await _ctx.Candidato.FindAsync(id);
        if (candidato != null)
        {
            _ctx.Candidato.Remove(candidato);
            await _ctx.SaveChangesAsync();
        }
    }


}