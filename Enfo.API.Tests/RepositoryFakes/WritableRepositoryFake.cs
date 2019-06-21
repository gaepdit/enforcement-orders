using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.API.Tests.RepositoryFakes
{
    public class WritableRepositoryFake<T> : ReadableRepositoryFake<T>, IAsyncWritableRepository<T>
        where T : BaseEntity
    {
        public WritableRepositoryFake(List<T> list) : base(list) { }

        public void Add(T item)
        {
            list.Add(item);
        }
        
        public Task CompleteAsync()
        {
            return Task.CompletedTask;
        }
    }
}
