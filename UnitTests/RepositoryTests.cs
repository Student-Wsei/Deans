using Domain.Entities;
using Infrastructure.Repositories;
using Moq;
using Xunit;

namespace UnitTests;

public class GenericRepositoryTests
{
    private readonly Mock<IStorage<Student>> _mockStorage;
    private readonly GenericRepositoryAsync<Student> _repository;

    public GenericRepositoryTests()
    {
        _mockStorage = new Mock<IStorage<Student>>();
        _repository = new GenericRepositoryAsync<Student>(_mockStorage.Object);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnStudent_WhenStudentExists()
    {
        // Given
        var studentId = Guid.NewGuid();
        var expectedStudent = new Student { Id = studentId, YearOfStudy = 1 };
        _mockStorage.Setup(s => s.FindByIdAsync(studentId)).ReturnsAsync(expectedStudent);

        // When
        var result = await _repository.FindByIdAsync(studentId);

        // Then
        Assert.Equal(expectedStudent, result);
        _mockStorage.Verify(s => s.FindByIdAsync(studentId), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnNull_WhenStudentDoesNotExist()
    {
        // Given
        var studentId = Guid.NewGuid();
        _mockStorage.Setup(s => s.FindByIdAsync(studentId)).ReturnsAsync((Student?)null);

        // When
        var result = await _repository.FindByIdAsync(studentId);

        // Then
        Assert.Null(result);
        _mockStorage.Verify(s => s.FindByIdAsync(studentId), Times.Once);
    }

    [Fact]
    public async Task FindAllAsync_ShouldReturnAllStudents()
    {
        // Given
        var students = new List<Student>
        {
            new Student { Id = Guid.NewGuid(), YearOfStudy = 1 },
            new Student { Id = Guid.NewGuid(), YearOfStudy = 2 }
        };
        _mockStorage.Setup(s => s.FindAllAsync()).ReturnsAsync(students);

        // When
        var result = await _repository.FindAllAsync();

        // Then
        Assert.Equal(students, result);
        _mockStorage.Verify(s => s.FindAllAsync(), Times.Once);
    }

    [Fact]
    public async Task FindPagedAsync_ShouldReturnPagedResult()
    {
        // Given
        var students = new List<Student>
        {
            new Student { Id = Guid.NewGuid(), YearOfStudy = 1 },
            new Student { Id = Guid.NewGuid(), YearOfStudy = 2 },
            new Student { Id = Guid.NewGuid(), YearOfStudy = 3 }
        };
        _mockStorage.Setup(s => s.FindAllAsync()).ReturnsAsync(students);
        int page = 1;
        int pageSize = 2;

        // When
        var result = await _repository.FindPagedAsync(page, pageSize);

        // Then
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(2, result.TotalPages);
        Assert.True(result.HasNext);
        Assert.False(result.HasPrevious);
        _mockStorage.Verify(s => s.FindAllAsync(), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldAddStudent()
    {
        // Given
        var student = new Student { YearOfStudy = 1 };

        // When
        var result = await _repository.AddAsync(student);

        // Then
        Assert.Equal(student, result);
        _mockStorage.Verify(s => s.AddAsync(student), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStudent()
    {
        // Given
        var student = new Student { Id = Guid.NewGuid(), YearOfStudy = 1 };

        // When
        var result = await _repository.UpdateAsync(student);

        // Then
        Assert.Equal(student, result);
        _mockStorage.Verify(s => s.UpdateAsync(student), Times.Once);
    }

    [Fact]
    public async Task RemoveByIdAsync_ShouldRemoveStudent()
    {
        // Given
        var studentId = Guid.NewGuid();

        // When
        await _repository.RemoveByIdAsync(studentId);

        // Then
        _mockStorage.Verify(s => s.RemoveByIdAsync(studentId), Times.Once);
    }
}