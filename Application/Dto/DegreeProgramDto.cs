using Domain.Entities;
using Domain.Enums;

namespace AppCore.Dto;

public sealed record DegreeProgramCreateDto
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Faculty { get; init; } = string.Empty;
    public DegreeType DegreeType { get; init; }
    public int DurationYears { get; init; }
    public int MinEctsForDiploma { get; init; }

    public DegreeProgram ToEntity()
    {
        return new DegreeProgram
        {
            Id = Guid.NewGuid(),
            Code = Code,
            Name = Name,
            Faculty = Faculty,
            DegreeType = DegreeType,
            DurationYears = DurationYears,
            MinEctsForDiploma = MinEctsForDiploma
        };
    }
}

public sealed record DegreeProgramReportDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Faculty { get; init; } = string.Empty;
    public int ActiveStudentCount { get; init; }
    public int GraduateCount { get; init; }
    public int TotalStudentCount { get; init; }

    public static DegreeProgramReportDto FromEntity(DegreeProgram program, int activeCount, int graduateCount, int totalCount)
    {
        return new DegreeProgramReportDto
        {
            Id = program.Id,
            Code = program.Code,
            Name = program.Name,
            Faculty = program.Faculty,
            ActiveStudentCount = activeCount,
            GraduateCount = graduateCount,
            TotalStudentCount = totalCount
        };
    }
}
