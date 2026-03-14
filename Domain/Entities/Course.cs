using Domain.Enums;

namespace Domain.Entities;

public class Course: EntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int EctsCredits { get; set; }
    public CompletionType CompletionType { get; set; }
    public Semester Semester { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public DegreeProgram? DegreeProgram { get; set; }
    public List<Student> Enrollments { get; set; } = new();
    public Lecturer? Instructor { get; set; }
}
