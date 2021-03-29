﻿using System.Collections.Generic;

namespace Enfo.Repository
{
    /// <summary>
    /// Authorization Roles for the application.
    /// </summary>
    public class UserRole
    {
        public string Name { get; }
        public string DisplayName { get; }
        public string Description { get; }

        private UserRole(string name, string displayName, string description)
        {
            (Name, DisplayName, Description) = (name, displayName, description);
            AllRoles.Add(this);
        }

        // This declaration must appear before the list of static instance types.
        public static List<UserRole> AllRoles { get; } = new();

        // Roles
        // These are the strings that are stored in the database. Avoid modifying these strings.
        public const string OrderAdministrator = "OrderAdministrator";

        public static UserRole OrderAdministratorRole { get; } = new(
            OrderAdministrator, "Order Administrator",
            "Users with the Order Administrator role are able to add and edit " +
            "enforcement orders.");

        public const string UserMaintenance = "UserMaintenance";

        public static UserRole UserMaintenanceRole { get; } = new(
            UserMaintenance, "User Maintenance",
            "Users with the User Maintenance role are able to add and remove " +
            "roles for other users.");

        public const string SiteMaintenance = "SiteMaintenance";

        public static UserRole SiteMaintenanceRole { get; } = new(
            SiteMaintenance, "Site Maintenance",
            "Users with the Site Maintenance role are able to update values in " +
            "lookup tables (drop-down lists).");
    }
}