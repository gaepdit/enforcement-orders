using System.Collections.Generic;

namespace Enfo.Repository
{
    /// <summary>
    /// Authorization Roles for the application.
    /// </summary>
    public static class UserRoles
    {
        // Roles
        // These are the strings that are stored in the database. Avoid modifying these strings. 
        public const string OrderAdministrator = "OrderAdministrator";
        public const string UserMaintenance = "UserMaintenance";
        public const string SiteMaintenance = "SiteMaintenance";

        public static readonly List<string> AllRoles = new()
            {UserMaintenance, SiteMaintenance, OrderAdministrator};

        public static string DisplayName(string role) =>
            role switch
            {
                OrderAdministrator => "Order Administrator",
                UserMaintenance => "User Maintenance",
                SiteMaintenance => "Site Maintenance",
                _ => role
            };

        public static string Description(string role) =>
            role switch
            {
                OrderAdministrator =>
                    "Users with the Order Administrator role are able to add and edit " +
                    "enforcement orders.",
                UserMaintenance =>
                    "Users with the User Maintenance role are able to add and remove " +
                    "roles for other users.",
                SiteMaintenance =>
                    "Users with the Site Maintenance role are able to update values in " +
                    "lookup tables (drop-down lists).",
                _ => DisplayName(role)
            };
    }
}