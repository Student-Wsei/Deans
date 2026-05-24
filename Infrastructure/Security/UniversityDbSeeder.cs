using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Security;

public class UniversityDbSeeder : IDataSeeder
{
    public int Order => 2;

    private readonly UniversityOfficeDbContext _context;
    private readonly ILogger<UniversityDbSeeder> _logger;

    public UniversityDbSeeder(
        UniversityOfficeDbContext context,
        ILogger<UniversityDbSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.DegreePrograms.AnyAsync())
        {
            _logger.LogInformation("Dane biznesowe już istnieją — pomijam seeding.");
            return;
        }

        var inf = new DegreeProgram
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Code = "INF",
            Name = "Informatyka",
            Faculty = "WMI",
            DegreeType = DegreeType.Bachelor,
            DurationYears = 3,
            MinEctsForDiploma = 180
        };
        var mat = new DegreeProgram
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Code = "MAT",
            Name = "Matematyka",
            Faculty = "WMI",
            DegreeType = DegreeType.Bachelor,
            DurationYears = 3,
            MinEctsForDiploma = 180
        };
        var eng = new DegreeProgram
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Code = "ENG",
            Name = "Filologia angielska",
            Faculty = "WNO",
            DegreeType = DegreeType.Master,
            DurationYears = 2,
            MinEctsForDiploma = 120
        };

        var year = new AcademicYear
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            YearFrom = 2026,
            YearTo = 2027,
            Name = "2026/2027",
            IsActive = true
        };

        await _context.DegreePrograms.AddRangeAsync(inf, mat, eng);
        await _context.AcademicYears.AddAsync(year);

        await _context.Person.AddRangeAsync(
            new Student
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0001"),
                FirstName = "Adam",
                LastName = "Nowak",
                NationalId = "0123456789",
                Email = new EmailAddress("adam.nowak@example.com"),
                StudentId = "S1001",
                YearOfStudy = 1,
                DegreeProgram = inf,
                EnrollmentYear = year,
                Status = StudentStatus.Active
            },
            new Student
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                FirstName = "Ewa",
                LastName = "Kowalska",
                NationalId = "9876543210",
                Email = new EmailAddress("ewa.kowalska@example.com"),
                StudentId = "S1002",
                YearOfStudy = 2,
                DegreeProgram = mat,
                EnrollmentYear = year,
                Status = StudentStatus.Active
            },
            new Student
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0003"),
                FirstName = "Piotr",
                LastName = "Wiśniewski",
                NationalId = "1111222233",
                Email = new EmailAddress("piotr.w@example.com"),
                StudentId = "S1003",
                YearOfStudy = 1,
                DegreeProgram = eng,
                EnrollmentYear = year,
                Status = StudentStatus.Active
            },
            new Student
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0004"),
                FirstName = "Anna",
                LastName = "Wójcik",
                NationalId = "4444555566",
                Email = new EmailAddress("anna.w@example.com"),
                StudentId = "S1004",
                YearOfStudy = 3,
                DegreeProgram = inf,
                EnrollmentYear = year,
                Status = StudentStatus.Active
            }
        );

        await _context.SaveChangesAsync();
        _logger.LogInformation("Dodano stopnie i studentów.");

        var java = new Lecturer
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0001"),
            FirstName = "Jan",
            LastName = "Kowalski",
            NationalId = "1111111111",
            Email = new EmailAddress("jan.kowalski@example.com"),
            Title = "dr",
            Faculty = "WMI"
        };
        var mNowak = new Lecturer
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0002"),
            FirstName = "Maria",
            LastName = "Nowak",
            NationalId = "2222222222",
            Email = new EmailAddress("maria.nowak@example.com"),
            Title = "prof.",
            Faculty = "WMI"
        };

        var prog = new Course
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0001"),
            Code = "INF-101",
            Name = "Programowanie obiektowe",
            EctsCredits = 6,
            CompletionType = CompletionType.Exam,
            Semester = Semester.Winter,
            AcademicYear = year,
            DegreeProgram = inf,
            Instructor = java
        };
        var alg = new Course
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0002"),
            Code = "INF-102",
            Name = "Algorytmy i struktury danych",
            EctsCredits = 5,
            CompletionType = CompletionType.Exam,
            Semester = Semester.Summer,
            AcademicYear = year,
            DegreeProgram = inf,
            Instructor = mNowak
        };
        var calculus = new Course
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0003"),
            Code = "MAT-201",
            Name = "Analiza matematyczna",
            EctsCredits = 6,
            CompletionType = CompletionType.Exam,
            Semester = Semester.Winter,
            AcademicYear = year,
            DegreeProgram = mat,
            Instructor = java
        };

        await _context.Person.AddRangeAsync(java, mNowak);
        await _context.Courses.AddRangeAsync(prog, alg, calculus);

        await _context.SaveChangesAsync();
        _logger.LogInformation("Dodano wykładowców i kursy.");
    }
}
