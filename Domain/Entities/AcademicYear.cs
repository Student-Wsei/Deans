namespace Domain.Entities;

public class AcademicYear: EntityBase
{
    public int YearFrom { get; set; }
    public int YearTo { get; set; }
    public bool IsActive { get; set; }
    public string Name { get; set; }
}