using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class InMemoryCourseRepository : MemoryGenericRepository<Course>, ICourseRepository
{
}
