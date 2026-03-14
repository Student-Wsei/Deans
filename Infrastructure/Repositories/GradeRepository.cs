using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class GradeRepository : GenericRepositoryAsync<Grade>, IGradeRepository
{
    public GradeRepository(IStorage<Grade> storage) : base(storage) { }
}
