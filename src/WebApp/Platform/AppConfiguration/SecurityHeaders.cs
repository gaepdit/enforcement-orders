using Enfo.WebApp.Platform.Settings;

namespace Enfo.WebApp.Platform.AppConfiguration;

internal static class SecurityHeaders
{
    private static readonly string DatadogReportUri =
        $"https://browser-intake-us3-datadoghq.com/api/v2/logs?" +
        $"dd-api-key={AppSettings.DataDogSettings.ClientToken}" +
        $"&dd-evp-origin=content-security-policy&ddsource=csp-report" +
        $"&ddtags=service%3Aenfo%2Cenv%3A{AppSettings.ShortEnv}%2Cversion%3A{AppSettings.SimpleVersion}";

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

        if (string.IsNullOrEmpty(AppSettings.DataDogSettings.ClientToken)) return;
        policies.AddReportingEndpoints(builder => builder.AddEndpoint("csp-endpoint", DatadogReportUri));
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void CspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().None();
        builder.AddObjectSrc().None();
        builder.AddScriptSrc().Self()
            .From("https://www.datadoghq-browser-agent.com/us3/v6/")
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
            .From("https://browser-intake-us3-datadoghq.com")
            .From("https://api.raygun.com")
            .From("https://api.raygun.io");
        builder.AddFontSrc().Self();
        builder.AddFormAction().Self()
            .From("https://*.okta.com")
            .From("https://login.microsoftonline.com");
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();
        builder.AddWorkerSrc()
            .From("https://www.datadoghq-browser-agent.com/us3/v6/");
        builder.AddReportTo("csp-endpoint");
    }
#pragma warning restore S1075
}
