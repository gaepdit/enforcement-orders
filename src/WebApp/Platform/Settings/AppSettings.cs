using JetBrains.Annotations;

namespace Enfo.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string Version { get; private set; }
    public static Support Support { get; } = new();
    public static EntraIdPhaseOut EntraIdPhaseOut { get; } = new();
    public static Raygun RaygunSettings { get; } = new();
    public static DataDog DataDogSettings { get; } = new();
    public static string OrgNotificationsApiUrl { get; private set; }

    public record Raygun
    {
        public string ApiKey { get; [UsedImplicitly] init; }
    }

    public record DataDog
    {
        public string ClientToken { get; [UsedImplicitly] init; }
        public string ApplicationId { get; [UsedImplicitly] init; }
    }
}

public record Support
{
    public string TechnicalSupportEmail { get; [UsedImplicitly] init; }
    public string TechnicalSupportSite { get; [UsedImplicitly] init; }
}

public record EntraIdPhaseOut
{
    public bool Enabled { get; [UsedImplicitly] init; }
    public DateOnly EndDate { get; [UsedImplicitly] init; }
}
