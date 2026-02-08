using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeopleIO.Domain.Contract;
using PeopleIO.Domain.Entity;
using PeopleIO.Infrastructure.Context;

namespace PeopleIO.Infrastructure.Repository;

public class CRUDRepository<T>: ICRUDRepository<T> where T : BaseEntity
{
    private readonly PeopleIoContext _ctx;
    private readonly DbSet<T> _dbSet;
    private readonly ILogger<CRUDRepository<T>> _logger;

    public CRUDRepository(PeopleIoContext ctx, ILogger<CRUDRepository<T>> logger)
    {
        _ctx = ctx;
        _logger = logger;
        _dbSet = _ctx.Set<T>();
    }
    
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync(ct);
    }

    public async Task<int> AddAsync(T entity, CancellationToken ct)
    {
        await _dbSet.AddAsync(entity, ct);
        return await _ctx.SaveChangesAsync(ct);
    }

    public async Task<int> UpdateAsync(Guid id, Action<T> updateAction, CancellationToken ct)
    {
        var existingENtity = await _dbSet.FindAsync(id);
        if (existingENtity is null)
        {
            return 0;
        }
        
        updateAction(existingENtity);
        return await _ctx.SaveChangesAsync(ct);
    }

    public Task<int> DeleteAsync(Guid id, CancellationToken ct) => 
        _dbSet.Where(e => e.Id == id).ExecuteDeleteAsync(ct);
}