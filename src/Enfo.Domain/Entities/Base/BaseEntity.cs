using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Enfo.Domain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }

        // TODO: Move to DbContext Audit Properties
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }

        // TODO: Move to DbContext Audit Properties
        /// <summary>
        /// See https://docs.microsoft.com/en-us/ef/core/modeling/concurrency
        /// </summary>
        [Timestamp]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Used for concurrency token")]
        public byte[] Timestamp { get; set; }
    }
}
