﻿using Microsoft.AspNetCore.Mvc;

namespace Enfo.Domain.LegalAuthorities.Resources;

public class LegalAuthorityCommand
{
    public LegalAuthorityCommand() { }

    public LegalAuthorityCommand(LegalAuthorityView item)
    {
        Id = item.Id;
        AuthorityName = item.AuthorityName;
    }

    [HiddenInput]
    public int? Id { get; init; }

    [Required(ErrorMessage = "Legal Authority Name is required.")]
    [DisplayName("Legal Authority Name")]
    public string AuthorityName { get; set; }

    public void TrimAll()
    {
        AuthorityName = AuthorityName?.Trim();
    }
}
