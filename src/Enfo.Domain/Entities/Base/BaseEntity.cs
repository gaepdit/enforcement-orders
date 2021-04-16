using System;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// A random value that should change whenever an entity is persisted to the store
        /// </summary>
        [ConcurrencyCheck]
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
