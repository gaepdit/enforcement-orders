﻿namespace Enfo.Domain.Users.Entities;

/// <summary>
/// Authorization Roles for the application.
/// </summary>
public class UserRole
{
    public string DisplayName { get; }
    public string Description { get; }

    [UsedImplicitly]
    private UserRole(string name, string displayName, string description)
    {
        DisplayName = displayName;
        Description = description;
        AllRoles.Add(name, this);
    }

    // This declaration must appear before the list of static instance types.
    public static Dictionary<string, UserRole> AllRoles { get; } = new();

    // Roles
    // These are the strings that are stored in the database. Avoid modifying!
    public const string OrderAdministrator = nameof(OrderAdministrator);
    public const string UserMaintenance = nameof(UserMaintenance);
    public const string SiteMaintenance = nameof(SiteMaintenance);

    // These static UserRole objects are used for displaying role information in the UI.
    [UsedImplicitly]
    public static UserRole OrderAdministratorRole { get; } = new(
        OrderAdministrator,
        "Order Administrator",
        "Users with the Order Administrator role are able to add and edit enforcement orders."
    );

    [UsedImplicitly]
    public static UserRole UserMaintenanceRole { get; } = new(
        UserMaintenance,
        "User Maintenance",
        "Users with the User Maintenance role are able to add and remove roles for other users."
    );

    public static UserRole SiteMaintenanceRole { get; } = new(
        SiteMaintenance,
        "Site Maintenance",
        "Users with the Site Maintenance role are able to update values in lookup tables (drop-down lists)."
    );
}
