﻿using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities
{
    public class Add : PageModel
    {
        [BindProperty]
        public LegalAuthorityCreate Item { get; set; }

        public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

        [TempData]
        public int HighlightId { get; set; }

        private readonly ILegalAuthorityRepository _repository;
        public Add(ILegalAuthorityRepository repository) => _repository = repository;

        [UsedImplicitly]
        public static void OnGet()
        {
            // Method intentionally left empty.
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            Item.TrimAll();

            if (await _repository.NameExistsAsync(Item.AuthorityName))
            {
                ModelState.AddModelError("Item.AuthorityName", "The authority name entered already exists.");
            }

            if (!ModelState.IsValid) return Page();

            HighlightId = await _repository.CreateAsync(Item);
            TempData?.SetDisplayMessage(Context.Success, $"{Item.AuthorityName} successfully added.");
            return RedirectToPage("Index");
        }
    }
}