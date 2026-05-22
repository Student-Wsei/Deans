using System.Threading.Tasks;
using Domain;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.UnitOfWork;

public class EfUniversityUnitOfWork(
    IStudentRepository students,
    ILecturerRepository lecturers,
    IGradeRepository grades,
    ICourseRepository courses,
    IDegreeProgramRepository degreePrograms,
    UniversityOfficeDbContext context
) : IUniversityUnitOfWork
{
    public IStudentRepository Students => students;
    public ILecturerRepository Lecturers => lecturers;
    public IGradeRepository Grades => grades;
    public ICourseRepository Courses => courses;
    public IDegreeProgramRepository DegreePrograms => degreePrograms;

    public ValueTask DisposeAsync()
    {
        return context.DisposeAsync();
    }

    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public Task BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransactionAsync()
    {
        return context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return context.Database.RollbackTransactionAsync();
    }
}
