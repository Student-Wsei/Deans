using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class InMemoryDegreeProgramRepository : MemoryGenericRepository<DegreeProgram>, IDegreeProgramRepository
{
}
