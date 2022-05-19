using Enfo.WebApp.Platform.Settings;

namespace Enfo.WebApp.Platform.SecurityHeaders;

internal static class SecurityHeaders
{
    internal static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies.AddFrameOptionsDeny();
        policies.AddXssProtectionBlock();
        policies.AddContentTypeOptionsNoSniff();
        policies.AddReferrerPolicyStrictOriginWhenCrossOrigin();
        policies.RemoveServerHeader();
        policies.AddContentSecurityPolicy(builder => builder.CspBuilder());
        policies.AddCustomHeader("Reporting-Endpoints",
            $"default=\"https://report-to-api.raygun.com/reports?apikey={ApplicationSettings.Raygun.ApiKey}\",csp-endpoint=\"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}\"");
        policies.AddCustomHeader("Report-To",
            $"{{\"group\":\"default\",\"max_age\":10886400,\"endpoints\":[{{\"url\":\"https://report-to-api.raygun.com/reports?apikey={ApplicationSettings.Raygun.ApiKey}\"}}]}},{{\"group\":\"csp-endpoint\",\"max_age\":10886400,\"endpoints\":[{{\"url\":\"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}\"}}]}}");
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void CspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().None();
        builder.AddScriptSrc()
            .Self()
            .From("https://cdn.raygun.io/raygun4js/raygun.min.js")
            .WithHash256("k8lqom5XjWiHpIL9TqKQ7DpRVbQNTtRtBFIKZ0iQaBk=")
            .WithHash256("lyolOjFEpwMenK+1PNbcwjIW7ZjHzw+EN8xe4louCcE=")
            .WithHash256("Tui7QoFlnLXkJCSl1/JvEZdIXTmBttnWNxzJpXomQjg=")
            .WithHash256("HmCUK6tSuR+K9jivpONI9hAkeIeDysc9CTJlS6tLklo=")
            .WithHashTagHelper()
            .ReportSample();
        builder.AddStyleSrc()
            .Self()
            .WithHash256("wkAU1AW/h8YFx0XlzvpTllAKnFEO2tw8aKErs5a26LY=")
            .WithHash256("lyolOjFEpwMenK+1PNbcwjIW7ZjHzw+EN8xe4louCcE=")
            .ReportSample();
        builder.AddImgSrc().Self().Data();
        builder.AddConnectSrc().Self().From("https://api.raygun.io");
        builder.AddFontSrc().Self();
        builder.AddFormAction()
            .Self()
            .From("https://login.microsoftonline.com");
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();
        builder.AddReportUri()
            .To($"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}");
        builder.AddCustomDirective("report-to", "csp-endpoint");
    }
#pragma warning restore S1075
}
