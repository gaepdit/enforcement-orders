﻿using Enfo.Domain.Entities;
using Enfo.Domain.Utils;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Enfo.API.Resources
{
    public class LegalAuthorityUpdateResource
    {
        public bool Active { get; set; } = true;

        [DisplayName("Legal Authority")]
        [Required(ErrorMessage = "Legal Authority name is required")]
        public string AuthorityName { get; set; }
    }

    public static class LegalAuthorityExtension
    {
        public static void UpdateFrom(this LegalAuthority item, LegalAuthorityUpdateResource resource)
        {
            Check.NotNull(item, nameof(item));
            Check.NotNull(resource, nameof(resource));

            item.Active = resource.Active;
            item.AuthorityName = resource.AuthorityName;
            item.UpdatedDate = DateTime.Now;
        }
    }
}