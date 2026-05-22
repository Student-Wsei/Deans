using System;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Dto;
using Domain;
using Domain.Enums;

namespace Application.Services;

public class DegreeProgramService(IUniversityUnitOfWork unitOfWork) : IDegreeProgramService
{
    public async Task<DegreeProgramDto> AddAsync(DegreeProgramCreateDto dto)
    {
        var entity = dto.ToEntity();
        var saved = await unitOfWork.DegreePrograms.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return DegreeProgramDto.FromEntity(saved);
    }

    public async Task<DegreeProgramReportDto?> GetReportAsync(Guid id)
    {
        var program = await unitOfWork.DegreePrograms.FindByIdAsync(id);
        if (program == null) return null;

        var allStudents = await unitOfWork.Students.FindAllAsync();
        var programStudents = allStudents.Where(s => s.DegreeProgram?.Id == id).ToList();

        var activeCount = programStudents.Count(s => s.Status == StudentStatus.Active);
        var graduateCount = programStudents.Count(s => s.Status == StudentStatus.Graduate);

        return DegreeProgramReportDto.FromEntity(program, activeCount, graduateCount, programStudents.Count);
    }
}
