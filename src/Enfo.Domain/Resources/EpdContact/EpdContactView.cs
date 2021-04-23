using System.ComponentModel;
using Enfo.Domain.Resources.Address;
using Enfo.Domain.Utils;
using JetBrains.Annotations;

namespace Enfo.Domain.Resources.EpdContact
{
    public class EpdContactView
    {
        public EpdContactView([NotNull] Domain.Entities.EpdContact item)
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

        public int Id { get; }
        public bool Active { get; }

        [DisplayName("Contact Name")]
        public string ContactName { get; }

        public string Title { get; }
        public string Organization { get; }
        public AddressView Address { get; }
        public string Telephone { get; }
        public string Email { get; }
    }
}
