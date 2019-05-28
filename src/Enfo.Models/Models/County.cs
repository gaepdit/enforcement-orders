using System.ComponentModel.DataAnnotations;

namespace Enfo.Models
{
    public class County
    {
        public int Id {get;set;}

        [StringLength(20)]
        public string CountyName {get;set;}
    }
}