using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class MemoryLecturerRepository : MemoryGenericRepository<Person>, ILecturerRepository
{
    public MemoryLecturerRepository() : base()
    {
    }

    public new async Task<Lecturer?> FindByIdAsync(Guid id)
    {
        var person = await base.FindByIdAsync(id);
        return person as Lecturer;
    }

    public new async Task<IEnumerable<Lecturer>> FindAllAsync()
    {
        var all = await base.FindAllAsync();
        return all.OfType<Lecturer>();
    }

    public new async Task<PagedResult<Lecturer>> FindPagedAsync(int page, int pageSize)
    {
        var all = (await FindAllAsync()).ToList();
        var total = all.Count;
        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return new PagedResult<Lecturer>(items, total, page, pageSize);
    }

    public new async Task<Lecturer> AddAsync(Lecturer entity)
    {
        await base.AddAsync(entity);
        return entity;
    }

    public new async Task<Lecturer> UpdateAsync(Lecturer entity)
    {
        await base.UpdateAsync(entity);
        return entity;
    }

    public async Task<Lecturer?> GetLecturerForCourseAsync(Course course)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Lecturer>> GetLecturersByTitleAsync(string title)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Lecturer>> GetLecturersByFacultyAsync(string faculty)
    {
        throw new NotImplementedException();
    }
}
