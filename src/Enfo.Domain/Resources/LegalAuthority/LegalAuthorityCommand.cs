using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;

namespace Enfo.Domain.Resources.LegalAuthority
{
    public class LegalAuthorityCommand
    {
        public LegalAuthorityCommand() { }

        public LegalAuthorityCommand(LegalAuthorityView item) =>
            AuthorityName = item.AuthorityName;

        [Required(ErrorMessage = "Legal Authority Name is required.")]
        [DisplayName("Legal Authority Name")]
        public string AuthorityName { get; set; }

        private void TrimAll()
        {
            AuthorityName = AuthorityName?.Trim();
        }

        public async Task<ResourceSaveResult> TrySaveNew(ILegalAuthorityRepository repository)
        {
            TrimAll();

            var result = new ResourceSaveResult();

            if (await repository.NameExistsAsync(AuthorityName))
            {
                result.AddValidationError(nameof(AuthorityName),
                    $"The authority name entered ({AuthorityName}) already exists.");
            }

            if (result.IsValid)
            {
                result.NewId = await repository.CreateAsync(this);
                result.Success = true;
            }

            return result;
        }

        public async Task<ResourceUpdateResult<LegalAuthorityView>> TryUpdate(
            ILegalAuthorityRepository repository,
            int Id)
        {
            var result = new ResourceUpdateResult<LegalAuthorityView>
            {
                OriginalItem = await repository.GetAsync(Id)
            };

            if (result.OriginalItem is null || !result.OriginalItem.Active)
            {
                return result;
            }

            TrimAll();

            if (await repository.NameExistsAsync(AuthorityName, Id))
            {
                result.AddValidationError(nameof(AuthorityName),
                    $"The authority name entered ({AuthorityName}) already exists.");
            }

            if (result.IsValid)
            {
                await repository.UpdateAsync(Id, this);
                result.Success = true;
            }

            return result;
        }
    }
}
