namespace Enfo.Domain.Services;

public interface IErrorLogger
{
    Task LogErrorAsync(Exception exception, Dictionary<string, object> customData = null);
}
