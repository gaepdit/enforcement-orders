using Enfo.WebApp.Platform.Settings;

namespace Enfo.WebApp.Platform.SecurityHeaders;

internal static class SecurityHeaders
{
    private static readonly string ReportUri =
        $"https://report-to-api.raygun.com/reports?apikey={ApplicationSettings.RaygunClientSettings.ApiKey}";

    internal static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies.AddFrameOptionsDeny();
        policies.AddContentTypeOptionsNoSniff();
        policies.AddReferrerPolicyStrictOriginWhenCrossOrigin();
        policies.RemoveServerHeader();
        policies.AddContentSecurityPolicyReportOnly(builder => builder.CspBuilder());
        policies.AddCrossOriginOpenerPolicy(builder => builder.SameOrigin());
        policies.AddCrossOriginEmbedderPolicy(builder => builder.Credentialless());
        policies.AddCrossOriginResourcePolicy(builder => builder.SameSite());

        if (string.IsNullOrEmpty(ApplicationSettings.RaygunClientSettings.ApiKey)) return;
        policies.AddReportingEndpoints(builder => builder.AddEndpoint("csp-endpoint", ReportUri));
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void CspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().None();
        builder.AddScriptSrc().Self()
            .From("https://cdn.raygun.io/raygun4js/raygun.min.js")
            .WithHash256("Tui7QoFlnLXkJCSl1/JvEZdIXTmBttnWNxzJpXomQjg=") // Swagger UI inline script
            .WithHash256("ZfBxknfwMkoMSoaip4gXIEEtJwKW2s2WlmmCB03P704=") // Swagger UI inline script
            .WithHashTagHelper()
            .WithNonce()
            .ReportSample();
        builder.AddStyleSrc().Self()
            .WithHash256("wkAU1AW/h8YFx0XlzvpTllAKnFEO2tw8aKErs5a26LY=") // Swagger UI inline style
            .ReportSample();
        builder.AddImgSrc().Self().Data();
        builder.AddConnectSrc().Self()
            .From("https://api.raygun.com")
            .From("https://api.raygun.io");
        builder.AddFontSrc().Self();
        builder.AddFormAction().Self()
            .From("https://login.microsoftonline.com");
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();

        if (string.IsNullOrEmpty(ApplicationSettings.RaygunClientSettings.ApiKey)) return;
        builder.AddReportUri().To(ReportUri);
        builder.AddReportTo("csp-endpoint");
    }
#pragma warning restore S1075
}
