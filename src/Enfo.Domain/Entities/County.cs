using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Entities
{
    public class County : BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string CountyName { get; set; }
    }
}