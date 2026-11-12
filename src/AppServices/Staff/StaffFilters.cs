using Enfo.Domain.Identity;

namespace Enfo.AppServices.Staff;

public static class StaffFilters
{
    public static IQueryable<ApplicationUser> ApplyFilter(
        this IQueryable<ApplicationUser> userQuery, StaffSearchDto spec) =>
        userQuery.FilterByName(spec.Name).FilterByEmail(spec.Email);

#pragma warning disable CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons
    // The 'StringComparison' method overload is incompatible with Entity Framework.
    private static IQueryable<ApplicationUser> FilterByName(
        this IQueryable<ApplicationUser> query, string? name) =>
        string.IsNullOrWhiteSpace(name)
            ? query
            : query.Where(user => user.GivenName.ToLower().Contains(name.ToLower())
                                  || user.FamilyName.ToLower().Contains(name.ToLower()));
#pragma warning restore CA1862

    private static IQueryable<ApplicationUser> FilterByEmail(
        this IQueryable<ApplicationUser> query, string? email) =>
        string.IsNullOrWhiteSpace(email) ? query : query.Where(user => user.Email == email);
}
