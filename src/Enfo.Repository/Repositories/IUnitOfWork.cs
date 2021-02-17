using System.Threading.Tasks;

namespace Enfo.Repository.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }
}
