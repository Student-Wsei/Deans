using System;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Dto;
using Domain;
using Domain.Entities;

namespace Application.Services;

public interface IStudentService
{
    Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size);
    Task<StudentSummaryDto?> GetById(Guid id);
    Task<Student> AddStudent(StudentCreateDto dto);
    Task<StudentSummaryDto?> UpdateStudent(Guid id, StudentUpdateDto dto);
    Task<StudentSummaryDto?> UpdateStudentStatus(Guid id, Domain.Enums.StudentStatus status);
    Task<GradeDto> AddGrade(Guid studentId, GradeCreateDto dto);
    Task<IEnumerable<GradeDto>> GetGrades(Guid studentId);
    Task<GradeDto?> UpdateGrade(Guid studentId, Guid gradeId, GradeUpdateDto dto);
}
