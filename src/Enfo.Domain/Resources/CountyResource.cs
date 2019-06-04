using Enfo.Domain.Models;

namespace Enfo.Domain.Resources
{
    public class CountyResource
    {
        public int Id { get; set; }

        public string CountyName { get; set; }

        public CountyResource() { }

        public CountyResource(County county)
        {
            if (county != null)
            {
                Id = county.Id;
                CountyName = county.CountyName;
            }
        }
    }
}