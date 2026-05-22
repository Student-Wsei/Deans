using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfLecturerRepository(UniversityOfficeDbContext context)
    : EfGenericRepository<Lecturer>(context.Lecturers), ILecturerRepository
{
    public async Task<Lecturer?> GetLecturerForCourseAsync(Course course)
    {
        return await context.Lecturers
            .FirstOrDefaultAsync(l => l.TaughtCourses.Any(c => c.Id == course.Id));
    }

    public async Task<List<Lecturer>> GetLecturersByTitleAsync(string title)
    {
        return await context.Lecturers
            .Where(l => l.Title == title)
            .ToListAsync();
    }

    public async Task<List<Lecturer>> GetLecturersByFacultyAsync(string faculty)
    {
        return await context.Lecturers
            .Where(l => l.Faculty == faculty)
            .ToListAsync();
    }
}
