namespace Enfo.Domain.Users;

/// <summary>
/// Class for listing and describing the application roles for use in the UI, etc.
/// </summary>
public class AppRole
{
    // User Roles available to the application for authorization.
    // These are the strings that are stored in the database. Avoid modifying these once set!

    public const string OrderAdministrator = nameof(OrderAdministrator);
    public const string UserMaintenance = nameof(UserMaintenance);
    public const string SiteMaintenance = nameof(SiteMaintenance);

    // App Role properties
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }

    private AppRole(string name, string displayName, string description)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        AllRoles.Add(name, this);
    }

    /// <summary>
    /// A Dictionary of all roles used by the app. The Dictionary key is a string containing 
    /// the <see cref="Microsoft.AspNetCore.Identity.IdentityRole.Name"/> of the role.
    /// (This declaration must appear before the list of static instance types.)
    /// </summary>
    public static Dictionary<string, AppRole> AllRoles { get; } = new();

    // These static Role objects are used for displaying role information in the UI.

    [UsedImplicitly]
    public static AppRole OrderAdministratorRole { get; } = new(
        OrderAdministrator,
        "Order Administrator",
        "Users with the Order Administrator role are able to add and edit enforcement orders."
    );

    [UsedImplicitly]
    public static AppRole UserMaintenanceRole { get; } = new(
        UserMaintenance,
        "User Maintenance",
        "Users with the User Maintenance role are able to add and remove roles for other users."
    );

    public static AppRole SiteMaintenanceRole { get; } = new(
        SiteMaintenance,
        "Site Maintenance",
        "Users with the Site Maintenance role are able to update values in lookup tables (drop-down lists)."
    );
}
