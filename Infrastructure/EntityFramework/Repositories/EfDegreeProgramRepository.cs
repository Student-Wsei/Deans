using Domain;
using Domain.Entities;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfDegreeProgramRepository(UniversityOfficeDbContext context)
    : EfGenericRepository<DegreeProgram>(context.DegreePrograms), IDegreeProgramRepository
{
}
