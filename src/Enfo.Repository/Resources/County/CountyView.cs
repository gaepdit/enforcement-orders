using System.ComponentModel;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Resources.County
{
    public class CountyView
    {
        public CountyView(Domain.Entities.County county)
        {
            Guard.NotNull(county, nameof(county));

            Id = county.Id;
            CountyName = county.CountyName;
        }

        public int Id { get; set; }

        [DisplayName("County")]
        public string CountyName { get; set; }
    }
}