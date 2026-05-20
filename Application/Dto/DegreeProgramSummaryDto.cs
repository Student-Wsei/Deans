using Domain.Entities;
using Domain.Enums;

namespace AppCore.Dto;

public sealed record DegreeProgramDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Faculty { get; init; } = string.Empty;
    public DegreeType DegreeType { get; init; }
    public int DurationYears { get; init; }
    public int MinEctsForDiploma { get; init; }

    public static DegreeProgramDto FromEntity(DegreeProgram program)
    {
        return new DegreeProgramDto
        {
            Id = program.Id,
            Code = program.Code,
            Name = program.Name,
            Faculty = program.Faculty,
            DegreeType = program.DegreeType,
            DurationYears = program.DurationYears,
            MinEctsForDiploma = program.MinEctsForDiploma
        };
    }
}
