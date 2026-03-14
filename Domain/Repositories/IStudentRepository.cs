using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Domain;

public interface IStudentRepository : IGenericRepositoryAsync<Student>
{
    Task<List<Student>> GetStudentsOnAcademicYearAsync(AcademicYear academicYear);
    Task<List<Student>> GetStudentsByDegreeProgramAsync(DegreeProgram degreeProgram);
    Task UpdateStudentStatusAsync(Guid studentId, StudentStatus status);
}
