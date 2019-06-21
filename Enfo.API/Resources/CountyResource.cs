using Enfo.Domain.Entities;

namespace Enfo.API.Resources
{
    public class CountyResource
    {
        public int Id { get; set; }
        public bool Active { get; set; }
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