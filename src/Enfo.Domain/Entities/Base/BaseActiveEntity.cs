namespace Enfo.Domain.Entities.Base;

public abstract class BaseActiveEntity : BaseEntity
{
    public bool Active { get; set; } = true;
}
