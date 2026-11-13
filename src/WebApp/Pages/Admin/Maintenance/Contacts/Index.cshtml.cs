using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.Identity;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts;

[Authorize]
public class IndexModel(IEpdContactRepository repository) : PageModel
{
    public IReadOnlyList<EpdContactView> Items { get; private set; } = null!;
    public static MaintenanceOption ThisOption => MaintenanceOption.EpdContact;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public int HighlightId { get; set; }

    public async Task OnGetAsync()
    {
        Items = await repository.ListAsync(true);
        IsSiteMaintainer = User.IsInRole(AppRole.SiteMaintenance);
    }
}
