using System.Collections.Generic;
using Enfo.Domain.Entities;

namespace Enfo.Domain.Repositories
{
    public class CreateEntityResult<T> where T : BaseEntity
    {
        public bool Success { get; private set; } = true;
        public T NewItem { get; set; }
        public Dictionary<string, string> ErrorMessages { get; }

        public CreateEntityResult()
        {
            ErrorMessages = new Dictionary<string, string>();
        }

        public CreateEntityResult(T item)
        {
            NewItem = item;
        }

        public CreateEntityResult(string key, string message)
        {
            Success = false;
            ErrorMessages = new Dictionary<string, string>
            {
                { key, message }
            };
        }

        public void AddErrorMessage(string key, string message)
        {
            Success = false;
            ErrorMessages.Add(key, message);
            NewItem = null;
        }
    }
}
