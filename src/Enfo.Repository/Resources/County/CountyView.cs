using System.ComponentModel;

namespace Enfo.Repository.Resources.County
{
    public class CountyView
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        [DisplayName("County")]
        public string CountyName { get; set; }
    }
}