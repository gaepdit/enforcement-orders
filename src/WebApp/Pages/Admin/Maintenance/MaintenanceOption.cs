namespace Enfo.WebApp.Pages.Admin.Maintenance;

public record MaintenanceOption
{
    public string SingularName { get; private init; }
    public string PluralName { get; private init; }

    private MaintenanceOption(string singularName, string pluralName)
    {
        SingularName = singularName;
        PluralName = pluralName;
    }

    public static MaintenanceOption EpdContact { get; } = new("EPD Contact", "EPD Contacts");

    public static MaintenanceOption LegalAuthority { get; } = new("Legal Authority", "Legal Authorities");
}
