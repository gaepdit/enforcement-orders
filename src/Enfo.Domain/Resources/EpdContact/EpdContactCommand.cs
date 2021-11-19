namespace Enfo.Domain.Resources.EpdContact;

public class EpdContactCommand
{
    public EpdContactCommand() { }

    public EpdContactCommand(EpdContactView item)
    {
        ContactName = item.ContactName;
        Email = item.Email;
        Organization = item.Organization;
        Telephone = item.Telephone;
        Title = item.Title;
        City = item.City;
        PostalCode = item.PostalCode;
        State = item.State;
        Street = item.Street;
        Street2 = item.Street2;
    }

    [DisplayName("Contact Full Name")]
    [Required(ErrorMessage = "Contact Name is required.")]
    [StringLength(250)]
    public string ContactName { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100)]
    public string Title { get; set; }

    [Required(ErrorMessage = "Organization is required.")]
    [StringLength(100)]
    public string Organization { get; set; }

    [DataType(DataType.PhoneNumber)]
    [StringLength(50)]
    [RegularExpression(ResourceRegex.Telephone, ErrorMessage = "Provide a valid phone number with area code.")]
    public string Telephone { get; set; }

    [DataType(DataType.EmailAddress)]
    [StringLength(100)]
    [RegularExpression(ResourceRegex.Email, ErrorMessage = "Provide a valid email address.")]
    public string Email { get; set; }

    [DisplayName("Street Address")]
    [Required(ErrorMessage = "Street Address is required.")]
    [StringLength(100)]
    public string Street { get; set; }

    [DisplayName("Apt / Suite / Other")]
    [StringLength(100)]
    public string Street2 { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50)]
    public string City { get; set; }

    [Required(ErrorMessage = "State is required.")]
    [StringLength(2)]
    [DefaultValue("GA")]
    public string State { get; set; } = "GA";

    [DisplayName("Postal Code")]
    [Required(ErrorMessage = "Postal Code is required.")]
    [DataType(DataType.PostalCode)]
    [StringLength(10)]
    [RegularExpression(ResourceRegex.PostalCode, ErrorMessage = "Provide a valid US ZIP Code.")]
    public string PostalCode { get; set; }

    public void TrimAll()
    {
        ContactName = ContactName?.Trim();
        Title = Title?.Trim();
        Organization = Organization?.Trim();
        Telephone = Telephone?.Trim();
        Email = Email?.Trim();
        Street = Street?.Trim();
        Street2 = Street2?.Trim();
        City = City?.Trim();
        State = State?.Trim();
        PostalCode = PostalCode?.Trim();
    }
}
