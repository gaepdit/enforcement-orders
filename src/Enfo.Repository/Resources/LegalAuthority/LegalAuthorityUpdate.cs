namespace Enfo.Repository.Resources.LegalAuthority
{
    public class LegalAuthorityUpdate : LegalAuthorityCreate
    {
        public bool Active { get; set; } = true;
    }
}