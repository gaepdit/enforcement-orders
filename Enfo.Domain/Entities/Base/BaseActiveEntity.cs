namespace Enfo.Domain.Entities
{
    public abstract class BaseActiveEntity : BaseEntity, IActive
    {
        public bool Active { get; set; } = true;
    }
}
