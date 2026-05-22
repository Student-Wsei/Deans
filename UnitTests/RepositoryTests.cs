using Application.Services;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Repositories;
using Xunit;

namespace UnitTests;

public class MemoryGenericRepositoryTest
{
    private IGenericRepositoryAsync<Student> _repo = new MemoryGenericRepository<Student>();

    [Fact]
    public async Task AddStudentToRepositoryTestAsync()
    {
        var expected = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = "Adam",
            LastName = "Kowalski",
            NationalId = "1234567890",
            Email = new Domain.ValueObjects.EmailAddress("adam@example.com"),
            YearOfStudy = 1,
            Status = StudentStatus.Active
        };
        await _repo.AddAsync(expected);
        var actual = await _repo.FindByIdAsync(expected.Id);
        Assert.Equal(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsNull_WhenNotFound()
    {
        var actual = await _repo.FindByIdAsync(Guid.NewGuid());
        Assert.Null(actual);
    }

    [Fact]
    public async Task FindAllAsync_ReturnsAllEntities()
    {
        var s1 = new Student { Id = Guid.NewGuid(), FirstName = "A" };
        var s2 = new Student { Id = Guid.NewGuid(), FirstName = "B" };
        await _repo.AddAsync(s1);
        await _repo.AddAsync(s2);
        var all = await _repo.FindAllAsync();
        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEntity()
    {
        var s = new Student { Id = Guid.NewGuid(), FirstName = "Old" };
        await _repo.AddAsync(s);
        s.FirstName = "New";
        await _repo.UpdateAsync(s);
        var actual = await _repo.FindByIdAsync(s.Id);
        Assert.Equal("New", actual?.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenEntityMissing()
    {
        var s = new Student { Id = Guid.NewGuid(), FirstName = "X" };
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repo.UpdateAsync(s));
    }

    [Fact]
    public async Task RemoveByIdAsync_DeletesEntity()
    {
        var s = new Student { Id = Guid.NewGuid(), FirstName = "X" };
        await _repo.AddAsync(s);
        await _repo.RemoveByIdAsync(s.Id);
        var actual = await _repo.FindByIdAsync(s.Id);
        Assert.Null(actual);
    }

    [Fact]
    public async Task RemoveByIdAsync_Throws_WhenMissing()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repo.RemoveByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task FindPagedAsync_ReturnsPage()
    {
        for (int i = 0; i < 5; i++)
        {
            await _repo.AddAsync(new Student { Id = Guid.NewGuid(), FirstName = $"S{i}" });
        }
        var page = await _repo.FindPagedAsync(1, 2);
        Assert.Equal(2, page.Items.Count);
        Assert.Equal(5, page.TotalCount);
        Assert.Equal(1, page.Page);
        Assert.Equal(3, page.TotalPages);
        Assert.True(page.HasNext);
        Assert.False(page.HasPrevious);
    }

    [Fact]
    public async Task AddAsync_AssignsId_WhenEmpty()
    {
        var s = new Student { FirstName = "NoId" };
        Assert.Equal(Guid.Empty, s.Id);
        var result = await _repo.AddAsync(s);
        Assert.NotEqual(Guid.Empty, result.Id);
    }
}

public class InMemoryStudentRepositoryTest
{
    private readonly MemoryStudentRepository _repo = new();
    private readonly AcademicYear _year = new() { Id = Guid.NewGuid(), YearFrom = 2024, YearTo = 2025, Name = "2024/2025" };
    private readonly DegreeProgram _program = new() { Id = Guid.NewGuid(), Code = "INF", Name = "Informatyka", Faculty = "WMI" };

    [Fact]
    public async Task GetStudentsOnAcademicYear_ReturnsMatching()
    {
        var s1 = new Student { Id = Guid.NewGuid(), EnrollmentYear = _year, FirstName = "A" };
        var s2 = new Student { Id = Guid.NewGuid(), EnrollmentYear = _year, FirstName = "B" };
        var s3 = new Student { Id = Guid.NewGuid(), EnrollmentYear = null, FirstName = "C" };
        await _repo.AddAsync(s1);
        await _repo.AddAsync(s2);
        await _repo.AddAsync(s3);
        var result = await _repo.GetStudentsOnAcademicYearAsync(_year);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetStudentsByDegreeProgram_ReturnsMatching()
    {
        var s1 = new Student { Id = Guid.NewGuid(), DegreeProgram = _program, FirstName = "A" };
        var s2 = new Student { Id = Guid.NewGuid(), DegreeProgram = null, FirstName = "B" };
        await _repo.AddAsync(s1);
        await _repo.AddAsync(s2);
        var result = await _repo.GetStudentsByDegreeProgramAsync(_program);
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateStudentStatus_ChangesStatus()
    {
        var s = new Student { Id = Guid.NewGuid(), Status = StudentStatus.Active, FirstName = "A" };
        await _repo.AddAsync(s);
        await _repo.UpdateStudentStatusAsync(s.Id, StudentStatus.Expelled);
        var actual = await _repo.FindByIdAsync(s.Id);
        Assert.Equal(StudentStatus.Expelled, actual?.Status);
    }
}

public class InMemoryLecturerRepositoryTest
{
    private readonly MemoryLecturerRepository _repo = new();

    [Fact]
    public async Task GetLecturerForCourse_ThrowsNotImplemented()
    {
        var course = new Course { Id = Guid.NewGuid() };
        await Assert.ThrowsAsync<NotImplementedException>(() => _repo.GetLecturerForCourseAsync(course));
    }
}

public class GradeExtensionsTest
{
    [Theory]
    [InlineData(GradeValue.Grade20, 2.0)]
    [InlineData(GradeValue.Grade30, 3.0)]
    [InlineData(GradeValue.Grade35, 3.5)]
    [InlineData(GradeValue.Grade40, 4.0)]
    [InlineData(GradeValue.Grade45, 4.5)]
    [InlineData(GradeValue.Grade50, 5.0)]
    public void Value_ReturnsDouble(GradeValue input, double expected)
    {
        Assert.Equal(expected, input.Value());
    }

    [Theory]
    [InlineData(GradeValue.Grade20, "Niedostateczny")]
    [InlineData(GradeValue.Grade30, "Dostateczny")]
    [InlineData(GradeValue.Grade35, "Dostateczny plus")]
    [InlineData(GradeValue.Grade40, "Dobry")]
    [InlineData(GradeValue.Grade45, "Dobry plus")]
    [InlineData(GradeValue.Grade50, "Bardzo dobry")]
    public void PolishName_ReturnsPolishName(GradeValue input, string expected)
    {
        Assert.Equal(expected, input.PolishName());
    }
}

public class EmailAddressTest
{
    [Theory]
    [InlineData("adam@wsei.edu.pl", "adam", "wsei.edu.pl")]
    [InlineData("Ewa.Kowalska@GMAIL.com", "Ewa.Kowalska", "gmail.com")]
    public void Constructor_ValidEmail_StoresUserAndDomain(string input, string expectedUser, string expectedDomain)
    {
        var email = new EmailAddress(input);
        Assert.Equal(expectedUser, email.User);
        Assert.Equal(expectedDomain, email.Domain);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    [InlineData("missing@")]
    [InlineData("@missing.com")]
    public void Constructor_InvalidEmail_Throws(string input)
    {
        Assert.Throws<ArgumentException>(() => new EmailAddress(input));
    }

    [Fact]
    public void TryParse_InvalidReturnsFalse()
    {
        Assert.False(EmailAddress.TryParse("not-an-email", out _));
    }

    [Fact]
    public void TryParse_ValidReturnsTrue()
    {
        Assert.True(EmailAddress.TryParse("a@b.com", out var email));
        Assert.NotNull(email);
    }

    [Fact]
    public void ImplicitStringConversion_Works()
    {
        var email = new EmailAddress("Test@Example.com");
        string s = email;
        Assert.Equal("Test@example.com", s);
    }
}

public class MemoryDegreeProgramServiceTest
{
    private readonly InMemoryDegreeProgramRepository _programRepo = new();
    private readonly MemoryStudentRepository _studentRepo = new();

    private DegreeProgramService BuildService()
    {
        var uow = new MemoryUniversityUnitOfWork(_studentRepo, new MemoryLecturerRepository(),
            new InMemoryGradeRepository(), new InMemoryCourseRepository(), _programRepo);
        return new DegreeProgramService(uow);
    }

    [Fact]
    public async Task AddAsync_CreatesProgram()
    {
        var service = BuildService();
        var dto = new AppCore.Dto.DegreeProgramCreateDto
        {
            Code = "INF",
            Name = "Informatyka",
            Faculty = "WMI",
            DegreeType = DegreeType.Bachelor,
            DurationYears = 3,
            MinEctsForDiploma = 180
        };
        var result = await service.AddAsync(dto);
        Assert.Equal("INF", result.Code);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task GetReportAsync_UnknownId_ReturnsNull()
    {
        var service = BuildService();
        var report = await service.GetReportAsync(Guid.NewGuid());
        Assert.Null(report);
    }
}
