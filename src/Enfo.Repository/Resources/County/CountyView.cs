using System.ComponentModel;
using Enfo.Repository.Utils;
using JetBrains.Annotations;

namespace Enfo.Repository.Resources.County
{
    public class CountyView
    {
        public CountyView([NotNull] Domain.Entities.County county)
        {
            Guard.NotNull(county, nameof(county));

            Id = county.Id;
            CountyName = county.CountyName;
        }

        public int Id { get; }

        [DisplayName("County")]
        public string CountyName { get; }
    }
}