using Domain.Enums;

namespace Domain.Entities;

public class Student: Person
{
    public string StudentId { get; set; } = string.Empty;
    public int YearOfStudy { get; set; }
    public DegreeProgram? DegreeProgram { get; set; }
    public AcademicYear? EnrollmentYear { get; set; }
    public StudentStatus Status { get; set; }
    public List<Grade> Grades { get; set; } = new();
}
