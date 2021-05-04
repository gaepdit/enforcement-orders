using System.Diagnostics.CodeAnalysis;

namespace Enfo.WebApp.Pages.Admin.Maintenance
{
    [SuppressMessage("ReSharper", "S3453",
        Justification = "See https://github.com/SonarSource/sonar-dotnet/issues/4282")]
    public class MaintenanceOption
    {
        public string SingularName { get; private init; }
        public string PluralName { get; private init; }

        private MaintenanceOption() { }

        public static MaintenanceOption Address { get; } =
            new() {SingularName = "EPD Address", PluralName = "EPD Addresses"};

        public static MaintenanceOption EpdContact { get; } =
            new() {SingularName = "EPD Contact", PluralName = "EPD Contacts"};

        public static MaintenanceOption LegalAuthority { get; } =
            new() {SingularName = "Legal Authority", PluralName = "Legal Authorities"};
    }
}