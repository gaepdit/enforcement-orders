﻿using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Specs;
using Enfo.WebApp.App;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages
{
    public class Search : PageModel
    {
        public EnforcementOrderSpec Spec { get; set; }
        public IPaginatedResult OrdersList { get; private set; }
        public bool ShowResults { get; private set; }

        // Select Lists
        public SelectList LegalAuthoritiesSelectList { get; private set; }

        private readonly IEnforcementOrderRepository _order;
        private readonly ILegalAuthorityRepository _legalAuthority;

        public Search(IEnforcementOrderRepository order, ILegalAuthorityRepository legalAuthority) =>
            (_order, _legalAuthority) = (order, legalAuthority);

        [UsedImplicitly]
        public async Task OnGetAsync()
        {
            // TODO: Remove after authentication is implemented
            ViewData["PageIsPublic"] = true;

            Spec = new EnforcementOrderSpec();
            LegalAuthoritiesSelectList = await GetLegalAuthoritiesSelectList();
        }

        [UsedImplicitly]
        public async Task OnGetSearchAsync(EnforcementOrderSpec spec, [FromQuery] int p = 1)
        {
            spec.TrimAll();
            Spec = spec;
            OrdersList = await _order.ListAsync(spec, new PaginationSpec(p, Pagination.PageSize));
            LegalAuthoritiesSelectList = await GetLegalAuthoritiesSelectList();
            ShowResults = true;
        }

        private async Task<SelectList> GetLegalAuthoritiesSelectList() =>
            new(await _legalAuthority.ListAsync(),
                nameof(LegalAuthorityView.Id),
                nameof(LegalAuthorityView.AuthorityName));
    }
}