﻿using System.Threading.Tasks;
using Enfo.Repository.Mapping;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.Address;
using Enfo.Repository.Resources.EpdContact;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts
{
    public class Edit : PageModel
    {
        [BindProperty]
        public EpdContactUpdate Item { get; set; }

        [BindProperty]
        public int Id { get; set; }

        [TempData]
        public int HighlightId { get; set; }

        public bool InactiveAddress { get; private set; }

        public SelectList AddressSelectList { get; private set; }

        public static MaintenanceOption ThisOption { get; } = MaintenanceOption.EpdContact;

        private readonly IEpdContactRepository _repository;
        private readonly IAddressRepository _address;

        public Edit(IEpdContactRepository repository, IAddressRepository address) =>
            (_repository, _address) = (repository, address);

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

            Item = EpdContactMapping.ToEpdContactUpdate(originalItem);
            Id = id.Value;
            InactiveAddress = !originalItem.Address.Active;
            await PopulateSelectListsAsync();
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

            Item.TrimAll();

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

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
            TempData?.SetDisplayMessage(Context.Success, $"{ThisOption.SingularName} successfully updated.");
            return RedirectToPage("Index");
        }

        private async Task PopulateSelectListsAsync() =>
            AddressSelectList = new SelectList(await _address.ListAsync(), nameof(AddressView.Id),
                nameof(AddressView.AsLinearString));
    }
}