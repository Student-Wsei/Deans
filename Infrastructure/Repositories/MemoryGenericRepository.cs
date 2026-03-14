using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class MemoryGenericRepository<T> : IGenericRepositoryAsync<T> where T : EntityBase
{
    protected Dictionary<Guid, T> _data = new();

    public Task<T?> FindByIdAsync(Guid id)
    {
        _data.TryGetValue(id, out var value);
        return Task.FromResult(value);
    }

    public Task<IEnumerable<T>> FindAllAsync()
    {
        IEnumerable<T> values = _data.Select(kv => kv.Value);
        return Task.FromResult(values);
    }

    public async Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        IEnumerable<T> all = await FindAllAsync();
        int total = System.Linq.Enumerable.Count(all);
        var items = System.Linq.Enumerable.Skip(all, (page - 1) * pageSize).Take(pageSize).ToList();
        return new PagedResult<T>(items, total, page, pageSize);
    }

    public Task<T> AddAsync(T entity)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(T entity)
    {
        if (entity.Id == Guid.Empty)
        {
            throw new InvalidOperationException("Cannot update entity with empty Id");
        }
        if (!_data.ContainsKey(entity.Id))
        {
            throw new InvalidOperationException($"Entity with id {entity.Id} not found");
        }
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task RemoveByIdAsync(Guid id)
    {
        if (!_data.ContainsKey(id))
        {
            throw new InvalidOperationException($"Entity with id {id} not found");
        }
        _data.Remove(id);
        return Task.CompletedTask;
    }
}
