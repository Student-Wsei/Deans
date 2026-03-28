using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/students")]
public class StudentsController(IStudentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllStudents([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        return Ok(await service.FindAllStudentsPaged(page, size));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(System.Guid id)
    {
        var result = await service.FindStudentById(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] AppCore.Dto.StudentCreateDto dto)
    {
        var created = await service.CreateStudent(dto);
        return Created($"/api/students/{created}", created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(System.Guid id, [FromBody] AppCore.Dto.StudentUpdateDto dto)
    {
        var updated = await service.UpdateStudent(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStudentStatus(System.Guid id, [FromQuery] Domain.Enums.StudentStatus status)
    {
        var updated = await service.UpdateStudentStatus(id, status);
        if (updated == null) return NotFound();
        return Ok(updated);
    }
}
