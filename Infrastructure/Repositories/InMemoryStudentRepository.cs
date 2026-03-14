using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repositories;

public class InMemoryStudentRepository : MemoryGenericRepository<Student>, IStudentRepository
{
    public async Task<List<Student>> GetStudentsOnAcademicYearAsync(AcademicYear academicYear)
    {
        var all = await FindAllAsync();
        return all.Where(s => s.EnrollmentYear?.Id == academicYear.Id).ToList();
    }

    public async Task<List<Student>> GetStudentsByDegreeProgramAsync(DegreeProgram degreeProgram)
    {
        var all = await FindAllAsync();
        return all.Where(s => s.DegreeProgram?.Id == degreeProgram.Id).ToList();
    }

    public async Task UpdateStudentStatusAsync(Guid studentId, StudentStatus status)
    {
        var student = await FindByIdAsync(studentId);
        if (student == null)
        {
            throw new InvalidOperationException($"Student with id {studentId} not found");
        }
        student.Status = status;
        await UpdateAsync(student);
    }
}
