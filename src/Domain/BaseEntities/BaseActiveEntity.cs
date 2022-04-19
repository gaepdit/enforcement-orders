namespace Enfo.Domain.BaseEntities;

public abstract class BaseActiveEntity : BaseEntity
{
    public bool Active { get; set; } = true;
}
