using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class DegreeProgramRepository : GenericRepositoryAsync<DegreeProgram>, IDegreeProgramRepository
{
    public DegreeProgramRepository(IStorage<DegreeProgram> storage) : base(storage) { }
}
