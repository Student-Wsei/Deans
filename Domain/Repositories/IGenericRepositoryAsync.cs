using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain;

public interface IGenericRepositoryAsync<T> where T : EntityBase
{
    Task<T?> FindByIdAsync(Guid id);
    Task<IEnumerable<T>> FindAllAsync();
    Task<PagedResult<T>> FindPagedAsync(int page, int pageSize);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task RemoveByIdAsync(Guid id);
}

public interface IGenericRepositoryAsync<T, TKey>
    where T : class
    where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
{
    Task<T?> FindByIdAsync(TKey id);
    Task<IEnumerable<T>> FindAllAsync();
    Task<PagedResult<T>> FindPagedAsync(int page, int pageSize);
    Task RemoveByIdAsync(TKey id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(TKey id, T o);
}
