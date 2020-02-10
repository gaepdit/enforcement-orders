using System.Collections.Generic;
using Enfo.Domain.Entities;

namespace Enfo.Domain.Repositories
{
    public class CreateEntityResult<T> where T : BaseEntity
    {
        public bool Success { get; private set; } = true;
        public T NewItem { get; private set; }
        public Dictionary<string, string> ErrorMessages { get; } = new Dictionary<string, string>();

        public CreateEntityResult() { }

        public CreateEntityResult(T item) =>
            AddItem(item);

        public CreateEntityResult(string key, string message) =>
            AddErrorMessage(key, message);

        public void AddErrorMessage(string key, string message)
        {
            Success = false;
            NewItem = null;
            ErrorMessages.Add(key, message);
        }

        public void AddItem(T item)
        {
            Success = true;
            NewItem = item;
            ErrorMessages.Clear();
        }
    }
}
