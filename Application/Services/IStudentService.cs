using System;
using System.Threading.Tasks;
using AppCore.Dto;
using Domain;

namespace Application.Services;

public interface IStudentService
{
    Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size);
    Task<StudentDetailDto?> FindStudentById(Guid id);
    Task<StudentDetailDto> CreateStudent(StudentCreateDto dto);
    Task<StudentDetailDto?> UpdateStudent(Guid id, StudentUpdateDto dto);
    Task<StudentDetailDto?> UpdateStudentStatus(Guid id, Domain.Enums.StudentStatus status);
}
