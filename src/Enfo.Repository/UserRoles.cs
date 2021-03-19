using System.Collections.Generic;

namespace Enfo.Repository
{
    /// <summary>
    /// Authorization Roles for the application.
    /// </summary>
    public static class UserRoles
    {
        /// Roles
        /// These are the strings that are stored in the database. Avoid modifying these strings. 
        public const string UserMaintenance = "UserMaintenance";

        public const string SiteMaintenance = "SiteMaintenance";
        public const string OrderAdministrator = "OrderAdministrator";

        public static readonly List<string> AllRoles = new()
            {UserMaintenance, SiteMaintenance, OrderAdministrator};

        public static string DisplayName(string role) =>
            role switch
            {
                UserMaintenance => "User Maintenance",
                SiteMaintenance => "Site Maintenance",
                OrderAdministrator => "Order Administrator",
                _ => role
            };

        public static string Description(string role) =>
            role switch
            {
                UserMaintenance =>
                    "Users with the User Maintenance role are able to add and remove " +
                    "roles for other users.",
                SiteMaintenance =>
                    "Users with the Site Maintenance role are able to update values in " +
                    "lookup tables (drop-down lists).",
                OrderAdministrator =>
                    "Users with the Order Administrator role are able to add and edit " +
                    "enforcement orders.",
                _ => DisplayName(role)
            };
    }
}