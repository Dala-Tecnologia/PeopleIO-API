using System.Linq.Expressions;
using PeopleIO.Domain.Entity;

namespace PeopleIO.Domain.Contract;

public interface ICRUDRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct, params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct, params Expression<Func<T, object>>[] includes);
    Task<int> AddAsync(T entity, CancellationToken ct);
    Task<int> UpdateAsync(Guid id,Action<T> updateAction, CancellationToken ct);
    Task<int> DeleteAsync(Guid id, CancellationToken ct); 
}