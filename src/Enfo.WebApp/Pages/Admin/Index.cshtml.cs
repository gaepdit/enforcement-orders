using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin;

[Authorize]
public class Index : PageModel
{
    public IReadOnlyList<EnforcementOrderSummaryView> CurrentProposedOrders { get; private set; }
    public IReadOnlyList<EnforcementOrderSummaryView> RecentExecutedOrders { get; private set; }
    public IReadOnlyList<EnforcementOrderAdminSummaryView> PendingOrders { get; private set; }
    public IReadOnlyList<EnforcementOrderAdminSummaryView> DraftOrders { get; private set; }
    public DisplayMessage Message { get; private set; }

    private readonly IEnforcementOrderRepository _repository;
    public Index(IEnforcementOrderRepository repository) => _repository = repository;

    public async Task OnGetAsync()
    {
        CurrentProposedOrders = await _repository.ListCurrentProposedEnforcementOrdersAsync();
        RecentExecutedOrders = await _repository.ListRecentlyExecutedEnforcementOrdersAsync();
        PendingOrders = await _repository.ListPendingEnforcementOrdersAsync();
        DraftOrders = await _repository.ListDraftEnforcementOrdersAsync();
        Message = TempData?.GetDisplayMessage();
    }

    public async Task<IActionResult> OnGetFindAsync(string find)
    {
        if (string.IsNullOrWhiteSpace(find)) return RedirectToPage("Index");

        var spec = new EnforcementOrderAdminSpec { OrderNumber = find };
        var orders = await _repository.ListAdminAsync(spec, new PaginationSpec(1, 1));

        if (orders.TotalCount == 1 && orders.Items[0] != null)
        {
            return RedirectToPage("Details", new { orders.Items[0].Id });
        }

        return RedirectToPage("/Admin/Search", "search", new { OrderNumber = find }, "search-results");
    }
}
