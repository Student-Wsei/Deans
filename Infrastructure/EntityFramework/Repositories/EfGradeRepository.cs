using Domain;
using Domain.Entities;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfGradeRepository(UniversityOfficeDbContext context)
    : EfGenericRepository<Grade>(context.Grades), IGradeRepository
{
}
