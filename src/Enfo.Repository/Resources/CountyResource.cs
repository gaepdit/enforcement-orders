using System.ComponentModel;
using Enfo.Domain.Entities;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Resources
{
    public class CountyResource
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        [DisplayName("County")]
        public string CountyName { get; set; }

        public CountyResource(County item)
        {
            Guard.NotNull(item, nameof(item));

            Id = item.Id;
            CountyName = item.CountyName;
        }
    }
}
