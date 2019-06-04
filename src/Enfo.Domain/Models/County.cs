using System.ComponentModel.DataAnnotations;

namespace Enfo.Domain.Models
{
    public class County : BaseModel
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string CountyName { get; set; }
    }
}