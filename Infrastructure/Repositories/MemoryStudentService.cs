using System;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Dto;
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

    public async Task<StudentDetailDto?> FindStudentById(Guid id)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        return student == null ? null : StudentDetailDto.FromEntity(student);
    }

    public async Task<StudentDetailDto> CreateStudent(StudentCreateDto dto)
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
        var student = StudentCreateDto.ToEntity(dto, program, year);
        await unitOfWork.Students.AddAsync(student);
        await unitOfWork.SaveChangesAsync();
        return StudentDetailDto.FromEntity(student);
    }

    public async Task<StudentDetailDto?> UpdateStudent(Guid id, StudentUpdateDto dto)
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
        return StudentDetailDto.FromEntity(student);
    }

    public async Task<StudentDetailDto?> UpdateStudentStatus(Guid id, StudentStatus status)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student == null) return null;
        student.Status = status;
        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();
        return StudentDetailDto.FromEntity(student);
    }
}
