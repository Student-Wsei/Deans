using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class InMemoryLecturerRepository : MemoryGenericRepository<Lecturer>, ILecturerRepository
{
    public async Task<Lecturer?> GetLecturerForCourseAsync(Course course)
    {
        var all = await FindAllAsync();
        return all.FirstOrDefault(l => l.TaughtCourses.Any(c => c.Id == course.Id));
    }

    public async Task<List<Lecturer>> GetLecturersByTitleAsync(string title)
    {
        var all = await FindAllAsync();
        return all.Where(l => l.Title == title).ToList();
    }

    public async Task<List<Lecturer>> GetLecturersByFacultyAsync(string faculty)
    {
        var all = await FindAllAsync();
        return all.Where(l => l.Faculty == faculty).ToList();
    }
}
