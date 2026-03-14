using System.Collections.Generic;
using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class LecturerRepository : GenericRepositoryAsync<Lecturer>, ILecturerRepository
{
    public LecturerRepository(IStorage<Lecturer> storage) : base(storage) { }

    public async Task<Lecturer?> GetLecturerForCourseAsync(Course course)
    {
        var all = await FindAllAsync();
        var lecturer = all.FirstOrDefault(l => l.TaughtCourses.Any(c => c.Id == course.Id));
        return lecturer;
    }

    public async Task<List<Lecturer>> GetLecturersByTitleAsync(string title)
    {
        var all = await FindAllAsync();
        var filtered = all.Where(l => l.Title == title).ToList();
        return filtered;
    }

    public async Task<List<Lecturer>> GetLecturersByFacultyAsync(string faculty)
    {
        var all = await FindAllAsync();
        var filtered = all.Where(l => l.Faculty == faculty).ToList();
        return filtered;
    }
}
