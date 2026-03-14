namespace Domain.Entities;

public class Lecturer: Person
{
    public string Title { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public List<Course> TaughtCourses { get; set; } = new();
}
