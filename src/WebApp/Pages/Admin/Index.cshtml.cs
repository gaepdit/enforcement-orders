using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;

namespace Enfo.WebApp.Pages.Admin;

[Authorize]
public class Index(IEnforcementOrderRepository repository) : PageModel
{
    public IReadOnlyList<EnforcementOrderSummaryView> CurrentProposedOrders { get; private set; }
    public IReadOnlyList<EnforcementOrderSummaryView> RecentExecutedOrders { get; private set; }
    public IReadOnlyList<EnforcementOrderAdminSummaryView> PendingOrders { get; private set; }
    public IReadOnlyList<EnforcementOrderAdminSummaryView> DraftOrders { get; private set; }
    public DisplayMessage Message { get; private set; }

    public async Task OnGetAsync()
    {
        CurrentProposedOrders = await repository.ListCurrentProposedEnforcementOrdersAsync();
        RecentExecutedOrders = await repository.ListRecentlyExecutedEnforcementOrdersAsync();
        PendingOrders = await repository.ListPendingEnforcementOrdersAsync();
        DraftOrders = await repository.ListDraftEnforcementOrdersAsync();
        Message = TempData?.GetDisplayMessage();
    }

    public async Task<IActionResult> OnGetFindAsync(string find)
    {
        if (string.IsNullOrWhiteSpace(find)) return RedirectToPage("Index");

        var spec = new EnforcementOrderAdminSpec { OrderNumber = find };
        var orders = await repository.ListAdminAsync(spec, new PaginationSpec(1, 1));

        if (orders.TotalCount == 1 && orders.Items[0] != null)
        {
            return RedirectToPage("Details", new { orders.Items[0].Id });
        }

        return RedirectToPage("/Admin/Search", "search", new { OrderNumber = find }, "search-results");
    }
}
