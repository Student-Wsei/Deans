using System;
using System.Threading.Tasks;
using AppCore.Dto;
using Domain.Entities;

namespace Application.Services;

public interface IDegreeProgramService
{
    Task<DegreeProgramDto> AddAsync(DegreeProgramCreateDto dto);
    Task<DegreeProgramReportDto?> GetReportAsync(Guid id);
}
