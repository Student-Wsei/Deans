using System;
using System.Threading.Tasks;
using AppCore.Authorization;
using AppCore.Dto;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/students")]
public class StudentsController(IStudentService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
    public async Task<IActionResult> GetAllStudents([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        return Ok(await service.FindAllStudentsPaged(page, size));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudent(Guid id)
    {
        var dto = await service.GetById(id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppCore.Dto.StudentCreateDto dto)
    {
        var entity = await service.AddStudent(dto);
        var result = new AppCore.Dto.StudentSummaryDto
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            StudentId = entity.StudentId,
            ProgramName = entity.DegreeProgram?.Name ?? string.Empty,
            YearOfStudy = entity.YearOfStudy,
            Status = entity.Status
        };
        return CreatedAtAction(nameof(GetStudent), new { id = entity.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AppCore.Dto.StudentUpdateDto dto)
    {
        var result = await service.UpdateStudent(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStudentStatus(Guid id, [FromQuery] Domain.Enums.StudentStatus status)
    {
        var updated = await service.UpdateStudentStatus(id, status);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpPost("{studentId:guid}/grades")]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddGrade(
        [FromRoute] Guid studentId,
        [FromBody] GradeCreateDto dto)
    {
        var note = await service.AddGrade(studentId, dto);
        return CreatedAtAction(
            nameof(GetGrades),
            new { studentId },
            note);
    }

    [HttpGet("{studentId:guid}/grades")]
    [ProducesResponseType(typeof(IEnumerable<GradeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrades([FromRoute] Guid studentId)
    {
        var student = await service.GetById(studentId);
        if (student == null) return NotFound();
        var grades = await service.GetGrades(studentId);
        return Ok(grades);
    }

    [HttpPut("{studentId:guid}/grades/{gradeId:guid}")]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGrade(
        [FromRoute] Guid studentId,
        [FromRoute] Guid gradeId,
        [FromBody] GradeUpdateDto dto)
    {
        var updated = await service.UpdateGrade(studentId, gradeId, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }
}
