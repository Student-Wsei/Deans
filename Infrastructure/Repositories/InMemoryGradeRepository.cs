using Domain;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class InMemoryGradeRepository : MemoryGenericRepository<Grade>, IGradeRepository
{
}
