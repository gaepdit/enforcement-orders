using System.Collections.Generic;

namespace Enfo.Domain.Validation
{
    public class ResourceValidationResult
    {
        public bool IsValid { get; private set; } = true;
        public Dictionary<string, string> ErrorMessages { get; } = new();

        public void AddErrorMessage(string key, string message)
        {
            IsValid = false;
            ErrorMessages.Add(key, message);
        }
    }
}
