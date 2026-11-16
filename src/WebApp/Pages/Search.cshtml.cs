using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.AppConfiguration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages;

public class Search(IEnforcementOrderRepository order, ILegalAuthorityRepository legalAuthority) : PageModel
{
    public EnforcementOrderSpec Spec { get; set; } = null!;
    public PaginatedResult<EnforcementOrderSummaryView> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);
    public SelectList LegalAuthoritiesSelectList { get; private set; } = null!;
    public bool ShowResults { get; private set; }

    public async Task OnGetAsync() => await PopulateSelectListsAsync();

    public async Task OnGetSearchAsync(EnforcementOrderSpec spec, [FromQuery] int p = 1,
        CancellationToken token = default)
    {
        Spec = spec.TrimAll();
        SearchResults = await order.ListAsync(Spec, new PaginationSpec(p, GlobalConstants.PageSize), token);
        await PopulateSelectListsAsync();
        ShowResults = true;
    }

    private async Task<SelectList> PopulateSelectListsAsync() =>
        LegalAuthoritiesSelectList = new SelectList(await legalAuthority.ListAsync(),
            nameof(LegalAuthorityViewDto.Id), nameof(LegalAuthorityViewDto.AuthorityName));
}
