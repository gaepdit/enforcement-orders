﻿using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
using Enfo.Domain.Mapping;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities
{
    [Authorize(Roles = UserRole.SiteMaintenance)]
    public class Edit : PageModel
    {
        [BindProperty]
        public LegalAuthorityUpdate Item { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Id { get; set; }

        [TempData, UsedImplicitly]
        public int HighlightId { [UsedImplicitly] get; set; }

        public string OriginalName { get; private set; }
        public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

        private readonly ILegalAuthorityRepository _repository;
        public Edit(ILegalAuthorityRepository repository) => _repository = repository;

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var originalItem = await _repository.GetAsync(id.Value);
            if (originalItem == null) return NotFound("ID not found.");

            if (!originalItem.Active)
            {
                TempData?.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
                return RedirectToPage("Index");
            }

            Item = LegalAuthorityMapping.ToLegalAuthorityUpdate(originalItem);
            Id = id.Value;
            OriginalName = originalItem.AuthorityName;
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            var originalItem = await _repository.GetAsync(Id);
            if (originalItem == null) return NotFound();

            if (!originalItem.Active)
            {
                TempData?.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
                return RedirectToPage("Index");
            }

            OriginalName = originalItem.AuthorityName;

            Item.TrimAll();

            if (await _repository.NameExistsAsync(Item.AuthorityName, Id))
            {
                ModelState.AddModelError("Item.AuthorityName", "The authority name entered already exists.");
            }

            if (!ModelState.IsValid) return Page();

            try
            {
                await _repository.UpdateAsync(Id, Item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(Id)) return NotFound();
                throw;
            }

            HighlightId = Id;
            TempData?.SetDisplayMessage(Context.Success, $"{Item.AuthorityName} successfully updated.");
            return RedirectToPage("Index");
        }
    }
}