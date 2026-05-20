using System;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Dto;
using AppCore.Exceptions;
using Application.Services;
using Domain;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repositories;

public class MemoryStudentService(IUniversityUnitOfWork unitOfWork) : IStudentService
{
    public async Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size)
    {
        var paged = await unitOfWork.Students.FindPagedAsync(page, size);
        var items = paged.Items.Select(StudentSummaryDto.FromEntity).ToList();
        return new PagedResult<StudentSummaryDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
    }

    public async Task<StudentSummaryDto?> GetById(Guid id)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        return student == null ? null : StudentSummaryDto.FromEntity(student);
    }

    public async Task<Student> AddStudent(StudentCreateDto dto)
    {
        DegreeProgram? program = null;
        if (!string.IsNullOrWhiteSpace(dto.ProgramCode))
        {
            program = await unitOfWork.DegreePrograms.FindByIdAsync(Guid.Empty);
        }
        AcademicYear? year = null;
        if (dto.EnrollmentYearFrom > 0)
        {
            year = new AcademicYear
            {
                Id = Guid.NewGuid(),
                YearFrom = dto.EnrollmentYearFrom,
                YearTo = dto.EnrollmentYearFrom + 1,
                Name = $"{dto.EnrollmentYearFrom}/{dto.EnrollmentYearFrom + 1}"
            };
        }
        var entity = StudentCreateDto.ToEntity(dto, program, year);
        entity = await unitOfWork.Students.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task<StudentSummaryDto?> UpdateStudent(Guid id, StudentUpdateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student == null) return null;
        DegreeProgram? program = null;
        if (!string.IsNullOrWhiteSpace(dto.ProgramCode))
        {
            program = await unitOfWork.DegreePrograms.FindByIdAsync(Guid.Empty);
        }
        dto.ApplyTo(student, program);
        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();
        return StudentSummaryDto.FromEntity(student);
    }

    public async Task<StudentSummaryDto?> UpdateStudentStatus(Guid id, StudentStatus status)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student == null) return null;
        student.Status = status;
        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();
        return StudentSummaryDto.FromEntity(student);
    }

    public async Task<GradeDto> AddGrade(Guid studentId, GradeCreateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId);
        if (student == null)
        {
            throw new LecturerNotFoundException($"Student with id={studentId} not found!");
        }
        var course = await unitOfWork.Courses.FindByIdAsync(dto.CourseId);
        if (course == null)
        {
            throw new LecturerNotFoundException($"Course with id={dto.CourseId} not found!");
        }
        var lecturer = await unitOfWork.Lecturers.FindByIdAsync(dto.LecturerId);
        if (lecturer == null)
        {
            throw new LecturerNotFoundException($"Lecturer with id={dto.LecturerId} not found!");
        }
        var grade = dto.ToEntity(student, course, lecturer, null);
        await unitOfWork.Grades.AddAsync(grade);
        await unitOfWork.SaveChangesAsync();
        return GradeDto.FromEntity(grade);
    }

    public async Task<IEnumerable<GradeDto>> GetGrades(Guid studentId)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId);
        if (student == null)
        {
            throw new LecturerNotFoundException($"Student with id={studentId} not found!");
        }
        var allGrades = await unitOfWork.Grades.FindAllAsync();
        var studentGrades = allGrades.Where(g => g.Student != null && g.Student.Id == studentId);
        return studentGrades.Select(GradeDto.FromEntity).ToList();
    }

    public async Task<GradeDto?> UpdateGrade(Guid studentId, Guid gradeId, GradeUpdateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId);
        if (student == null) return null;
        var grade = await unitOfWork.Grades.FindByIdAsync(gradeId);
        if (grade == null) return null;
        if (grade.Student == null || grade.Student.Id != studentId) return null;
        dto.ApplyTo(grade);
        await unitOfWork.Grades.UpdateAsync(grade);
        await unitOfWork.SaveChangesAsync();
        return GradeDto.FromEntity(grade);
    }
}
