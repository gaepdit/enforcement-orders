﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EpdContact;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts
{
    public class Index : PageModel
    {
        public IReadOnlyList<EpdContactView> Items { get; private set; }
        public static MaintenanceOption ThisOption => MaintenanceOption.EpdContact;
        public DisplayMessage Message { get; private set; }

        [TempData]
        public int HighlightId { get; set; }

        private readonly IEpdContactRepository _repository;
        public Index(IEpdContactRepository repository) => _repository = repository;

        [UsedImplicitly]
        public async Task OnGetAsync()
        {
            Items = await _repository.ListAsync(true);
            Message = TempData?.GetDisplayMessage();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return Page();
            var item = await _repository.GetAsync(id.Value);
            if (item == null) return NotFound();

            try
            {
                await _repository.UpdateStatusAsync(id.Value, !item.Active);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(id.Value)) return NotFound();
                throw;
            }

            TempData?.SetDisplayMessage(Context.Success,
                $"{ThisOption.SingularName} successfully {(item.Active ? "deactivated" : "restored")}.");

            return RedirectToPage("Index");
        }
    }
}