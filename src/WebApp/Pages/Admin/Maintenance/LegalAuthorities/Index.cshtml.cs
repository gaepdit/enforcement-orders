using Enfo.Domain.Identity;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[Authorize]
public class IndexModel(ILegalAuthorityRepository repository) : PageModel
{
    public IReadOnlyList<LegalAuthorityView> Items { get; private set; } = null!;
    public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public int HighlightId { get; set; }

    public async Task OnGetAsync()
    {
        Items = await repository.ListAsync(true);
        IsSiteMaintainer = User.IsInRole(AppRole.SiteMaintenance);
    }
}
