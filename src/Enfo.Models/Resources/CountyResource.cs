using Enfo.Models.Models;

namespace Enfo.Models.Resources
{
    public class CountyResource
    {
        public int Id { get; set; }

        public string CountyName { get; set; }

        public CountyResource(County county)
        {
            Id = county.Id;
            CountyName = county.CountyName;
        }
    }
}