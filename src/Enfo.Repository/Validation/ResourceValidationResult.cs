using System.Collections.Generic;

namespace Enfo.Repository.Validation
{
    public class ResourceValidationResult
    {
        public bool IsValid { get; private set; } = true;
        public Dictionary<string, string> ErrorMessages { get; } = new Dictionary<string, string>();
        
        public void AddErrorMessage(string key, string message)
        {
            IsValid = false;
            ErrorMessages.Add(key, message);
        }
    }
}
