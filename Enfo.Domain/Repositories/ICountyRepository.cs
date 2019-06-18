using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;

namespace Enfo.Infrastructure.Repositories
{
    public interface ICountyRepository : IAsyncReadOnlyRepository<County> { }
}