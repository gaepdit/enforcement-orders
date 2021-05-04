﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Enfo.Domain.Resources.Users;
using Enfo.Domain.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Users
{
    [Authorize]
    public class Index : PageModel
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool ShowResults { get; set; }
        public List<UserView> SearchResults { get; set; }

        [UsedImplicitly]
        public static void OnGet()
        {
            // Method intentionally left empty.
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnGetSearchAsync([FromServices] IUserService userService,
            string name, string email)
        {
            if (!ModelState.IsValid) return Page();
            SearchResults = await userService.GetUsersAsync(name, email);
            ShowResults = true;
            return Page();
        }
    }
}