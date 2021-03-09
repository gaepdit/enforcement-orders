namespace Enfo.Domain.Entities
{
    public abstract class BaseActiveEntity : BaseEntity
    {
        public bool Active { get; set; } = true;
    }
}
