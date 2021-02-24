using System.Collections.Generic;
using Enfo.Repository.Resources.County;

namespace Enfo.Repository.Repositories
{
    public interface ICountyRepository 
    {
        CountyView Get(int id);
        IReadOnlyList<CountyView> List();
    }
}