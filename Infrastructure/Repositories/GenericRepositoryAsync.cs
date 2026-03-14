using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : BaseEntity
{
    private readonly IStorage<T> _storage;

    public GenericRepositoryAsync(IStorage<T> storage)
    {
        _storage = storage;
    }

    public async Task<T?> FindByIdAsync(Guid id)
    {
        return await _storage.FindByIdAsync(id);
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await _storage.FindAllAsync();
    }

    public async Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        var all = (await _storage.FindAllAsync()).AsQueryable();
        var totalCount = all.Count();
        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var result = new PagedResult<T>(items, totalCount, page, pageSize);
        return result;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _storage.AddAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        await _storage.UpdateAsync(entity);
        return entity;
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        await _storage.RemoveByIdAsync(id);
    }
}
