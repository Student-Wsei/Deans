using System;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Repositories;

public class MemoryUniversityUnitOfWork(
    IStudentRepository persons,
    ILecturerRepository companies,
    IGradeRepository organizations,
    ICourseRepository courses,
    IDegreeProgramRepository degreePrograms
) : IUniversityUnitOfWork
{
    public IStudentRepository Students => persons;
    public ILecturerRepository Lecturers => companies;
    public IGradeRepository Grades => organizations;
    public ICourseRepository Courses => courses;
    public IDegreeProgramRepository DegreePrograms => degreePrograms;

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }
}
