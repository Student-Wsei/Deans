using System.Collections.Generic;
using Domain;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repositories;

public class StudentRepository : GenericRepositoryAsync<Student>, IStudentRepository
{
    public StudentRepository(IStorage<Student> storage) : base(storage) { }

    public async Task<List<Student>> GetStudentsOnAcademicYearAsync(AcademicYear academicYear)
    {
        var all = await FindAllAsync();
        var filtered = all.Where(s => s.EnrollmentYear?.Id == academicYear.Id).ToList();
        return filtered;
    }

    public async Task<List<Student>> GetStudentsByDegreeProgramAsync(DegreeProgram degreeProgram)
    {
        var all = await FindAllAsync();
        var filtered = all.Where(s => s.DegreeProgram?.Id == degreeProgram.Id).ToList();
        return filtered;
    }

    public async Task UpdateStudentStatusAsync(Guid studentId, StudentStatus status)
    {
        var student = await FindByIdAsync(studentId);
        if (student != null)
        {
            student.Status = status;
            await UpdateAsync(student);
        }
    }
}
