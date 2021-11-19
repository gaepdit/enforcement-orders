namespace Enfo.Domain.Resources;

public abstract class ResourceCommandResult
{
    protected ResourceCommandResult()
    {
        Success = false;
        IsValid = true;
        ValidationErrors = new();
    }

    public bool Success { get; set; }
    public bool IsValid { get; private set; }

    public Dictionary<string, string> ValidationErrors { get; }

    public void AddValidationError(string key, string message)
    {
        IsValid = false;
        ValidationErrors.Add(key, message);
    }
}
