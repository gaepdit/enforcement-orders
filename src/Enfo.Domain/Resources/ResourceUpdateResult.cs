namespace Enfo.Domain.Resources;

public class ResourceUpdateResult<T> : ResourceCommandResult
{
    public T OriginalItem { get; set; }
}
