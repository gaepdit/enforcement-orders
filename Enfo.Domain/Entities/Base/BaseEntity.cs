﻿using System;

namespace Enfo.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
    }
}
