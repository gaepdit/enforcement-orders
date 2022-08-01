using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Platform.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages.Admin;

[Authorize]
public class Search : PageModel
{
    public EnforcementOrderAdminSpec Spec { get; set; }
    public PaginatedResult<EnforcementOrderAdminSummaryView> OrdersList { get; private set; }
    public bool ShowResults { get; private set; }

    // Select Lists
    public SelectList LegalAuthoritiesSelectList { get; private set; }

    private readonly IEnforcementOrderRepository _repository;
    private readonly ILegalAuthorityRepository _legalAuthority;

    public Search(IEnforcementOrderRepository repository, ILegalAuthorityRepository legalAuthority) =>
        (_repository, _legalAuthority) = (repository, legalAuthority);

    public async Task OnGetAsync()
    {
        Spec = new EnforcementOrderAdminSpec();
        LegalAuthoritiesSelectList = await GetLegalAuthoritiesSelectList();
    }

    public async Task OnGetSearchAsync(EnforcementOrderAdminSpec spec, [FromQuery] int p = 1)
    {
        spec.TrimAll();
        Spec = spec;
        OrdersList = await _repository.ListAdminAsync(spec, new PaginationSpec(p, GlobalConstants.PageSize));
        LegalAuthoritiesSelectList = await GetLegalAuthoritiesSelectList();
        ShowResults = true;
    }

    private async Task<SelectList> GetLegalAuthoritiesSelectList() =>
        new(await _legalAuthority.ListAsync(),
            nameof(LegalAuthorityView.Id),
            nameof(LegalAuthorityView.AuthorityName));
}
