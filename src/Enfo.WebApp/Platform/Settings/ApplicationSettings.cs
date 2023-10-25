using JetBrains.Annotations;

namespace Enfo.WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public const string RaygunSettingsSection = "RaygunSettings";
    public static RaygunClientSettings RaygunClientSettings { get; } = new();

    public const string LocalDevSettingsSection = "LocalDevSettings";
    public static LocalDevSettings LocalDevSettings { get; } = new();

    public static FileSystemSettings FileServiceSettings { get; } = new();
}

public class RaygunClientSettings
{
    public string ApiKey { get; [UsedImplicitly] init; }
}

public class LocalDevSettings
{
    public bool UseAzureAd { get; [UsedImplicitly] init; }
    public bool AuthenticatedUser { get; [UsedImplicitly] init; }
    public bool BuildLocalDb { get; [UsedImplicitly] init; }
    public bool UseSecurityHeadersLocally { get; [UsedImplicitly] init; }
}

public static class FileServiceConstants
{
    // File Service types
    public const string InMemory = nameof(InMemory);
    public const string FileSystem = nameof(FileSystem);
    public const string AzureBlobStorage = nameof(AzureBlobStorage);
}

public class FileSystemSettings
{
    // File Service (Set to one of the value in `FileServiceConstants`.)
    public string FileService { get; [UsedImplicitly] init; }

    // File System parameters
    public string FileSystemBasePath { get; [UsedImplicitly] set; }
    public string NetworkUsername { get; [UsedImplicitly] set; }
    public string NetworkDomain { get; [UsedImplicitly] set; }
    public string NetworkPassword { get; [UsedImplicitly] set; }

    // Azure Blob Storage parameters
    public string AzureAccountName { get; [UsedImplicitly] set; }
    public string BlobContainer { get; [UsedImplicitly] set; }
    public string BlobBasePath  { get; [UsedImplicitly] set; }
}
