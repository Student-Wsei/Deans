using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class CourseRepository : GenericRepositoryAsync<Course>, ICourseRepository
{
    public CourseRepository(IStorage<Course> storage) : base(storage) { }
}
