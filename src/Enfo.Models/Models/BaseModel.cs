using System;

namespace Enfo.Models.Models
{
    public abstract class BaseModel
    {
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
    }
}