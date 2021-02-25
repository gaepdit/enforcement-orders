using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Resources.Address;
using Enfo.Repository.Utils;

namespace Enfo.Repository.Resources.EpdContact
{
    public class EpdContactView
    {
        public EpdContactView(Domain.Entities.EpdContact item)
        {
            Guard.NotNull(item, nameof(item));

            Id = item.Id;
            Active = item.Active;
            ContactName = item.ContactName;
            Title = item.Title;
            Organization = item.Organization;
            Address = new AddressView(item.Address);
            Telephone = item.Telephone;
            Email = item.Email;
        }

        public int Id { get; set; }
        public bool Active { get; set; }

        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        public string Title { get; set; }
        public string Organization { get; set; }
        public AddressView Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
    }
}