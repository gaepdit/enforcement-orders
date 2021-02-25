using Enfo.Repository.Utils;

namespace Enfo.Repository.Resources.LegalAuthority
{
    public class LegalAuthorityView : LegalAuthorityUpdate
    {
        public LegalAuthorityView(Domain.Entities.LegalAuthority item)
        {
            Guard.NotNull(item, nameof(item));
            
            Id = item.Id;
            Active = item.Active;
            AuthorityName = item.AuthorityName;
        }
        public int Id { get; set; }
    }
}