using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.Services
{
    public abstract class BaseService
    {
        protected EnfoDbContext Context { get; }

        public BaseService(EnfoDbContext context)
        {
            Context = context;
        }
    }
}
