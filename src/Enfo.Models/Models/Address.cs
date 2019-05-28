using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Enfo.Models.Utils.StringFunctions;

namespace Enfo.Models
{
    public class Address : ModelBase
    {
        // Postal (mailable) addresses only

        [DisplayName("Street Address")]
        [StringLength(100)]
        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [DisplayName("Apt / Suite / Other")]
        [StringLength(100)]
        public string Street2 { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [DefaultValue("GA")]
        [StringLength(2)]
        public string State { get; set; } = "GA";

        [DisplayName("Postal Code")]
        [StringLength(10)]
        [RegularExpression("^\\d{5}(-\\d{4})?$", ErrorMessage = "Valid US Postal Code is required")]
        public string PostalCode { get; set; }

        public const string Country = "USA";

        public override string ToString()
        {
            return CompileAddressString();
        }

        private string CompileAddressString(string lineSeparator = ", ")
        {
            string cityState = ConcatNonEmptyStrings(new string[] { this.City, this.State }, ", ");
            string cityStateZip = ConcatNonEmptyStrings(new string[] { cityState, this.PostalCode }, " ");
            return ConcatNonEmptyStrings(new string[] { this.Street, this.Street2, cityStateZip }, lineSeparator);
        }

        public bool Active { get; set; } = true;
    }
}