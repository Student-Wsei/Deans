using System;
using System.Threading.Tasks;

namespace Domain;

public interface IUniversityUnitOfWork : IAsyncDisposable
{
    IStudentRepository Students { get; }
    ILecturerRepository Lecturers { get; }
    IGradeRepository Grades { get; }
    ICourseRepository Courses { get; }
    IDegreeProgramRepository DegreePrograms { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
