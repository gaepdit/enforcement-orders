using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Specs;
using Enfo.WebApp.Platform;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages.Admin
{
    public class Search : PageModel
    {
        public EnforcementOrderAdminSpec Spec { get; set; }
        public IPaginatedResult OrdersList { get; private set; }
        public bool ShowResults { get; private set; }

        // Select Lists
        public SelectList LegalAuthoritiesSelectList { get; private set; }

        private readonly IEnforcementOrderRepository _repository;
        private readonly ILegalAuthorityRepository _legalAuthority;

        public Search(IEnforcementOrderRepository repository, ILegalAuthorityRepository legalAuthority) =>
            (_repository, _legalAuthority) = (repository, legalAuthority);

        [UsedImplicitly]
        public async Task OnGetAsync()
        {
            Spec = new EnforcementOrderAdminSpec();
            LegalAuthoritiesSelectList = await GetLegalAuthoritiesSelectList();
        }

        [UsedImplicitly]
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
}