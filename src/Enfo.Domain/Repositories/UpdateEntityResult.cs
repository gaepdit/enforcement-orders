using System.Collections.Generic;

namespace Enfo.Domain.Repositories
{
    public class UpdateEntityResult
    {
        public bool Success { get; private set; } = true;
        public Dictionary<string, string> ErrorMessages { get; }

        public UpdateEntityResult()
        {
            ErrorMessages = new Dictionary<string, string>();
        }

        public UpdateEntityResult(string key, string message)
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
        }
    }
}
