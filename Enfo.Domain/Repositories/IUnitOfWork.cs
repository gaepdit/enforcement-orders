using System;
using System.Threading.Tasks;

namespace Enfo.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}