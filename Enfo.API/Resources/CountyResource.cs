using Enfo.Domain.Entities;

namespace Enfo.API.Resources
{
    public class CountyResource
    {
        public int Id { get; set; }

        public string CountyName { get; set; }

        public bool Active { get; set; } 

        public CountyResource() { }

        public CountyResource(County entity)
        {
            if (entity != null)
            {
                Id = entity.Id;
                Active = entity.Active;
                CountyName = entity.CountyName;
            }
        }
    }
}