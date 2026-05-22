using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfStudentRepository(UniversityOfficeDbContext context)
    : EfGenericRepository<Student>(context.Students), IStudentRepository
{
    public async Task<List<Student>> GetStudentsOnAcademicYearAsync(AcademicYear academicYear)
    {
        return await context.Students
            .Include(s => s.DegreeProgram)
            .Where(s => s.EnrollmentYear != null && s.EnrollmentYear.Id == academicYear.Id)
            .ToListAsync();
    }

    public async Task<List<Student>> GetStudentsByDegreeProgramAsync(DegreeProgram degreeProgram)
    {
        return await context.Students
            .Where(s => s.DegreeProgram != null && s.DegreeProgram.Id == degreeProgram.Id)
            .ToListAsync();
    }

    public async Task UpdateStudentStatusAsync(Guid studentId, StudentStatus status)
    {
        var student = await context.Students.FindAsync(studentId);
        if (student == null)
        {
            throw new InvalidOperationException($"Student with id {studentId} not found");
        }
        student.Status = status;
        context.Students.Update(student);
    }
}
