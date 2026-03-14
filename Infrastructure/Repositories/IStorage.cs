using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IStorage<T> where T : BaseEntity
{
    Task<T?> FindByIdAsync(Guid id);
    Task<IEnumerable<T>> FindAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveByIdAsync(Guid id);
}
