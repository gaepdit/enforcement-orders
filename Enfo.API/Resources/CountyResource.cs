using Enfo.Domain.Entities;
using Enfo.Domain.Utils;
using System.ComponentModel;

namespace Enfo.API.Resources
{
    public class CountyResource
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        [DisplayName("County")]
        public string CountyName { get; set; }

        public CountyResource(County item)
        {
            Check.NotNull(item, nameof(item));

            Id = item.Id;
            Active = item.Active;
            CountyName = item.CountyName;
        }
    }
}
