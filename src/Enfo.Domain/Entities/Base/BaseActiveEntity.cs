namespace Enfo.Domain.Entities
{
    public abstract class BaseActiveEntity : BaseEntity, IActiveEntity
    {
        public bool Active { get; set; } = true;
    }
}
