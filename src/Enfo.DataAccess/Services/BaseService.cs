using Enfo.DataAccess.Contexts;

namespace Enfo.DataAccess.Services
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
