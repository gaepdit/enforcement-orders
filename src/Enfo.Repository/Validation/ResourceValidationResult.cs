using System.Collections.Generic;

namespace Enfo.Repository.Validation
{
    public class ResourceValidationResult
    {
        public bool Success { get; private set; } = true;
        public Dictionary<string, string> ErrorMessages { get; } = new Dictionary<string, string>();
        
        public void AddErrorMessage(string key, string message)
        {
            Success = false;
            ErrorMessages.Add(key, message);
        }
    }
}
