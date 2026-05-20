using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace AppCore.Dto;

public sealed record StudentSummaryDto : PersonDto
{
    public string StudentId { get; init; } = string.Empty;
    public string ProgramName { get; init; } = string.Empty;
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }

    public static StudentSummaryDto FromEntity(Student student)
    {
        return new StudentSummaryDto
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            StudentId = student.StudentId,
            ProgramName = student.DegreeProgram?.Name ?? string.Empty,
            YearOfStudy = student.YearOfStudy,
            Status = student.Status
        };
    }
}

public sealed record StudentDetailDto : PersonDto
{
    public string StudentId { get; init; } = string.Empty;
    public string ProgramCode { get; init; } = string.Empty;
    public string ProgramName { get; init; } = string.Empty;
    public string EnrollmentYear { get; init; } = string.Empty;
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }
    public double GradePointAverage { get; init; }
    public int TotalEctsEarned { get; init; }
    public bool IsEligibleForDiploma { get; init; }

    public static StudentDetailDto FromEntity(Student student)
    {
        return new StudentDetailDto
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            StudentId = student.StudentId,
            ProgramCode = student.DegreeProgram?.Code ?? string.Empty,
            ProgramName = student.DegreeProgram?.Name ?? string.Empty,
            EnrollmentYear = student.EnrollmentYear?.Name ?? string.Empty,
            YearOfStudy = student.YearOfStudy,
            Status = student.Status,
            GradePointAverage = 0,
            TotalEctsEarned = 0,
            IsEligibleForDiploma = false
        };
    }
}

public sealed record StudentCreateDto : PersonCreateDto
{
    public string StudentId { get; init; } = string.Empty;
    public int YearOfStudy { get; init; } = 1;
    public string ProgramCode { get; init; } = string.Empty;
    public int EnrollmentYearFrom { get; init; }

    public static Student ToEntity(StudentCreateDto dto, DegreeProgram? program = null, AcademicYear? enrollmentYear = null)
    {
        return new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalId = dto.NationalId,
            Email = EmailAddress.Parse(dto.Email),
            StudentId = dto.StudentId,
            YearOfStudy = dto.YearOfStudy,
            DegreeProgram = program,
            EnrollmentYear = enrollmentYear,
            Status = StudentStatus.Active
        };
    }
}

public sealed record StudentUpdateDto : PersonDto
{
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }
    public string ProgramCode { get; init; } = string.Empty;

    public void ApplyTo(Student student, DegreeProgram? program = null)
    {
        student.FirstName = FirstName;
        student.LastName = LastName;
        student.Email = EmailAddress.Parse(Email);
        student.YearOfStudy = YearOfStudy;
        student.Status = Status;
        if (program != null)
        {
            student.DegreeProgram = program;
        }
    }
}
