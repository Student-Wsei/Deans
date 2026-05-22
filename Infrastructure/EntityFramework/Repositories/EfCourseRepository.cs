using Domain;
using Domain.Entities;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCourseRepository(UniversityOfficeDbContext context)
    : EfGenericRepository<Course>(context.Courses), ICourseRepository
{
}
