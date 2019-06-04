using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnfoDbContext context;

        public UnitOfWork(EnfoDbContext context)
        {
            this.context = context;
        }

        public Task CompleteAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}