using Domain.Enums;

namespace Domain.Entities;

public class Grade: EntityBase
{
    public Student Student { get; set; }
    public Course Course { get; set; }
    public DateTime Date { get; set; }
    public GradeType GradeType { get; set; }
    public Lecturer? Instructor { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public GradeValue GradeValue { get; set; }
}