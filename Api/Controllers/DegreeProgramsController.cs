using System;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/degree-programs")]
public class DegreeProgramsController(IDegreeProgramService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AppCore.Dto.DegreeProgramCreateDto dto)
    {
        var result = await service.AddAsync(dto);
        return Created($"/api/degree-programs/{result.Id}", result);
    }

    [HttpGet("{id:guid}/report")]
    public async Task<IActionResult> GetReport(Guid id)
    {
        var report = await service.GetReportAsync(id);
        if (report == null) return NotFound();
        return Ok(report);
    }
}
