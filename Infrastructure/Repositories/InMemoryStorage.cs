using System.Collections.Concurrent;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class InMemoryStorage<T> : IStorage<T> where T : BaseEntity
{
    private static readonly ConcurrentDictionary<Guid, T> _storage = new();

    public async Task<T?> FindByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var entity);
        return await Task.FromResult(entity);
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await Task.FromResult(_storage.Values.AsEnumerable());
    }

    public async Task AddAsync(T entity)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }
        _storage[entity.Id] = entity;
    }

    public async Task UpdateAsync(T entity)
    {
        if (_storage.ContainsKey(entity.Id))
        {
            _storage[entity.Id] = entity;
        }
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
    }
}
