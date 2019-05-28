using System;

namespace Enfo.Models
{
    public abstract class ModelBase
    {
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
    }
}