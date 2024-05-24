namespace Enfo.Domain.Users.Entities
{
    public static class UserDomainValidation
    {
        public static bool isValidEmailDomain(this string email) =>
            email.EndsWith("@dnr.ga.gov", StringComparison.CurrentCultureIgnoreCase);
    }
}
