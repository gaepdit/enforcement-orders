using System.ComponentModel;

namespace Enfo.API.QueryStrings
{
    public class ActiveItemFilter
    {
        [DefaultValue(false)]
        public bool IncludeInactive { get; set; } = false;
    }
}
