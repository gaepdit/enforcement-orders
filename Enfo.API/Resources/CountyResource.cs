using Enfo.Domain.Entities;
using System.ComponentModel;

namespace Enfo.API.Resources
{
    public class CountyResource
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        [DisplayName("County")]
        public string CountyName { get; set; }

        public CountyResource() { }
        public CountyResource(County item)
        {
            if (item != null)
            {
                Id = item.Id;
                Active = item.Active;
                CountyName = item.CountyName;
            }
        }
    }
}