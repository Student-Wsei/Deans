using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repositories;

public class MemoryStudentRepository : MemoryGenericRepository<Person>, IStudentRepository
{
    public MemoryStudentRepository() : base()
    {
        _data.Add(Guid.NewGuid(), new Student
        {
            FirstName = "Adam",
            LastName = "Nowak",
            NationalId = "0123456789",
            Email = "adam.nowak@example.com",
            StudentId = "S1001",
            YearOfStudy = 1,
            Status = StudentStatus.Active
        });
        _data.Add(Guid.NewGuid(), new Student
        {
            FirstName = "Ewa",
            LastName = "Kowalska",
            NationalId = "9876543210",
            Email = "ewa.kowalska@example.com",
            StudentId = "S1002",
            YearOfStudy = 2,
            Status = StudentStatus.Active
        });
    }

    public new async Task<Student?> FindByIdAsync(Guid id)
    {
        var person = await base.FindByIdAsync(id);
        return person as Student;
    }

    public new async Task<IEnumerable<Student>> FindAllAsync()
    {
        var all = await base.FindAllAsync();
        return all.OfType<Student>();
    }

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

    public new async Task<PagedResult<Student>> FindPagedAsync(int page, int pageSize)
    {
        var all = (await FindAllAsync()).ToList();
        var total = all.Count;
        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return new PagedResult<Student>(items, total, page, pageSize);
    }

    public new async Task<Student> AddAsync(Student entity)
    {
        await base.AddAsync(entity);
        return entity;
    }

    public new async Task<Student> UpdateAsync(Student entity)
    {
        await base.UpdateAsync(entity);
        return entity;
    }
}
