using Enfo.Domain.Entities;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.County;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Mapping
{
    public static class CountyMapping
    {
        public static CountyView ToCountyView(County item)
        {
            Guard.NotNull(item, nameof(item));

            return new CountyView()
            {
                Active = true,
                Id = item.Id,
                CountyName = item.CountyName,
            };
        }
    }
}