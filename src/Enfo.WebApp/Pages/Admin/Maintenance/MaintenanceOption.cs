namespace Enfo.WebApp.Pages.Admin.Maintenance
{
    public class MaintenanceOption
    {
        public string SingularName { get; private init; }
        public string PluralName { get; private init; }

        private MaintenanceOption() { }

        public static MaintenanceOption EpdContact { get; } =
            new() {SingularName = "EPD Contact", PluralName = "EPD Contacts"};

        public static MaintenanceOption LegalAuthority { get; } =
            new() {SingularName = "Legal Authority", PluralName = "Legal Authorities"};
    }
}