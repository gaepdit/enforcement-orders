using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Domain.Entities;
using Enfo.Domain.Utils;

namespace Enfo.API.Resources
{
    public class EpdContactResource
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        [DisplayName("Contact Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Contact Name is required")]
        public string ContactName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Organization is required")]
        public string Organization { get; set; }

        public virtual AddressResource Address { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public int? AddressId { get; set; }

        [RegularExpression("^\\D?(\\d{3})\\D?\\D?(\\d{3})\\D?(\\d{4})$",
            ErrorMessage = "Please enter valid a phone number")]
        [StringLength(50)]
        public string Telephone { get; set; }

        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$",
            ErrorMessage = "Please provide a valid email address")]
        [StringLength(100)]
        public string Email { get; set; }

        public EpdContactResource(EpdContact item)
        {
            Check.NotNull(item, nameof(item));

            Id = item.Id;
            Active = item.Active;
            ContactName = item.ContactName;
            Title = item.Title;
            Organization = item.Organization;
            Address = new AddressResource(item.Address);
            AddressId = item.AddressId;
            Telephone = item.Telephone;
            Email = item.Email;
        }
    }
}
