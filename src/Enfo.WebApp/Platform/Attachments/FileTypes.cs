namespace Enfo.WebApp.Platform.Attachments;

public static class FileTypes
{
    private static readonly Dictionary<string, string> FileContentTypes = new()
    {
        { ".txt", "text/plain" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".xls", "application/ms-excel" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".doc", "application/msword" },
        { ".pdf", "application/pdf" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".bmp", "image/bmp" },
        { ".gif", "image/gif" },
        { ".svg", "image/svg+xml" },
        { ".html", "text/html" },
        { ".htm", "text/html" },
        { ".rtf", "application/rtf" },
        { ".zip", "application/zip" },
        { ".csv", "text/csv" },
        { ".ppt", "application/vnd.ms-powerpoint" },
        { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
    };

    public static bool FileUploadAllowed(string fileExtension) =>
        FileContentTypes.ContainsKey(fileExtension.ToLower());

    public static string GetContentType(string fileExtension) =>
        FileContentTypes.ContainsKey(fileExtension.ToLower())
            ? FileContentTypes[fileExtension.ToLower()]
            : "application/octet-stream";
}
