﻿using Enfo.Domain.BaseEntities;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.Utils;

namespace Enfo.Domain.EpdContacts.Entities;

public class EpdContact : IdentifiedEntity, IAuditable
{
    public EpdContact() { }

    public EpdContact(EpdContactCommand resource) => ApplyUpdate(resource);

    [Required]
    [StringLength(250)]
    public string ContactName { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(100)]
    public string Organization { get; set; }

    [StringLength(50)]
    public string Telephone { get; set; }

    [StringLength(100)]
    public string Email { get; set; }

    // Postal (mailable) addresses only

    [Required]
    [StringLength(100)]
    public string Street { get; set; }

    [StringLength(100)]
    public string Street2 { get; set; }

    [Required]
    [StringLength(50)]
    public string City { get; set; }

    [Required]
    [DefaultValue("GA")]
    [StringLength(2)]
    public string State { get; set; } = "GA";

    [StringLength(10)]
    public string PostalCode { get; set; }

    public bool Active { get; set; } = true;

    public void ApplyUpdate(EpdContactCommand resource)
    {
        Guard.NotNull(resource);

        ContactName = Guard.NotNullOrWhiteSpace(resource.ContactName);
        Email = Guard.RegexMatch(resource.Email, ResourceRegex.Email);
        Organization = Guard.NotNullOrWhiteSpace(resource.Organization);
        Telephone = Guard.RegexMatch(resource.Telephone, ResourceRegex.Telephone);
        Title = Guard.NotNullOrWhiteSpace(resource.Title);
        City = Guard.NotNullOrWhiteSpace(resource.City);
        PostalCode = Guard.RegexMatch(resource.PostalCode, ResourceRegex.PostalCode);
        State = Guard.NotNullOrWhiteSpace(resource.State);
        Street = Guard.NotNullOrWhiteSpace(resource.Street);
        Street2 = resource.Street2;
    }
}
