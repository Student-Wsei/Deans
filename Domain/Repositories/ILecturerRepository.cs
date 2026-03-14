using System.Collections.Generic;
using Domain.Entities;

namespace Domain;

public interface ILecturerRepository : IGenericRepositoryAsync<Lecturer>
{
    Task<Lecturer?> GetLecturerForCourseAsync(Course course);
    Task<List<Lecturer>> GetLecturersByTitleAsync(string title);
    Task<List<Lecturer>> GetLecturersByFacultyAsync(string faculty);
}
