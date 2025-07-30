using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Platform.AppConfiguration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages;

public class Search(IEnforcementOrderRepository order, ILegalAuthorityRepository legalAuthority)
    : PageModel
{
    public EnforcementOrderSpec Spec { get; set; }
    public PaginatedResult<EnforcementOrderSummaryView> OrdersList { get; private set; }
    public bool ShowResults { get; private set; }

    // Select Lists
    public SelectList LegalAuthoritiesSelectList { get; private set; }

    public async Task OnGetAsync()
    {
        Spec = new EnforcementOrderSpec();
        LegalAuthoritiesSelectList = await GetLegalAuthoritiesSelectList();
    }

    public async Task OnGetSearchAsync(EnforcementOrderSpec spec, [FromQuery] int p = 1)
    {
        spec.TrimAll();
        Spec = spec;
        OrdersList = await order.ListAsync(spec, new PaginationSpec(p, GlobalConstants.PageSize));
        LegalAuthoritiesSelectList = await GetLegalAuthoritiesSelectList();
        ShowResults = true;
    }

    private async Task<SelectList> GetLegalAuthoritiesSelectList() =>
        new(await legalAuthority.ListAsync(),
            nameof(LegalAuthorityView.Id),
            nameof(LegalAuthorityView.AuthorityName));
}
