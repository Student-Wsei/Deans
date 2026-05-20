using Domain.Entities;
using Domain.Enums;

namespace AppCore.Dto;

public sealed record GradeDto
{
    public Guid Id { get; init; }
    public Guid StudentId { get; init; }
    public Guid CourseId { get; init; }
    public Guid? LecturerId { get; init; }
    public Guid? AcademicYearId { get; init; }
    public DateTime Date { get; init; }
    public GradeType GradeType { get; init; }
    public GradeValue GradeValue { get; init; }

    public static GradeDto FromEntity(Grade grade)
    {
        return new GradeDto
        {
            Id = grade.Id,
            StudentId = grade.Student?.Id ?? Guid.Empty,
            CourseId = grade.Course?.Id ?? Guid.Empty,
            LecturerId = grade.Instructor?.Id,
            AcademicYearId = grade.AcademicYear?.Id,
            Date = grade.Date,
            GradeType = grade.GradeType,
            GradeValue = grade.GradeValue
        };
    }
}

public sealed record GradeCreateDto
{
    public Guid CourseId { get; init; }
    public Guid LecturerId { get; init; }
    public Guid AcademicYearId { get; init; }
    public DateTime Date { get; init; }
    public GradeType GradeType { get; init; }
    public GradeValue GradeValue { get; init; }

    public Grade ToEntity(Student student, Course course, Lecturer lecturer, AcademicYear year)
    {
        return new Grade
        {
            Id = Guid.NewGuid(),
            Student = student,
            Course = course,
            Instructor = lecturer,
            AcademicYear = year,
            Date = Date,
            GradeType = GradeType,
            GradeValue = GradeValue
        };
    }
}

public sealed record GradeUpdateDto
{
    public DateTime Date { get; init; }
    public GradeType GradeType { get; init; }
    public GradeValue GradeValue { get; init; }

    public void ApplyTo(Grade grade)
    {
        grade.Date = Date;
        grade.GradeType = GradeType;
        grade.GradeValue = GradeValue;
    }
}
