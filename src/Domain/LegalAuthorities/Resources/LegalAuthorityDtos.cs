namespace Enfo.Domain.LegalAuthorities.Resources;

public record LegalAuthorityCreateDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Legal Authority Name is required.")]
    [DisplayName("Legal Authority Name")]
    public string AuthorityName { get; init; }
}

public record LegalAuthorityUpdateDto : LegalAuthorityCreateDto
{
    public bool Active { get; init; }
}
