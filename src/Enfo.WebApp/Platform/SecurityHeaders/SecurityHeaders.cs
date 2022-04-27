using Enfo.WebApp.Platform.Settings;

namespace Enfo.WebApp.Platform.SecurityHeaders;

internal static class SecurityHeaders
{
    internal static void AddEnfoSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies.AddFrameOptionsDeny();
        policies.AddXssProtectionBlock();
        policies.AddContentTypeOptionsNoSniff();
#if !DEBUG
        policies.AddStrictTransportSecurityMaxAge((int)TimeSpan.FromDays(730).TotalSeconds);
#endif
        policies.AddReferrerPolicyStrictOriginWhenCrossOrigin();
        policies.RemoveServerHeader();
        policies.AddContentSecurityPolicy(builder => builder.EnfoCspBuilder());
        policies.AddCustomHeader("Report-Endpoints",
            $"raygun=\"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}\"");
        policies.AddCustomHeader("Report-To",
            $"{{\"group\":\"raygun\",\"max_age\":2592000,\"endpoints\":[{{\"url\":\"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}\"}}]}}");
        policies.AddCustomHeader("NEL",
            "{\"report_to\": \"network-errors\", \"max_age\": 2592000}");
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void EnfoCspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().Self();
        builder.AddScriptSrc()
            .Self()
            .From(new[]
            {
                "https://cdn.raygun.io/raygun4js/raygun.min.js",
                "https://trunk.georgia.gov/ga_legacy/js_header",
                "https://trunk.georgia.gov/ga_legacy/js_footer",
            })
            .WithHash256("k8lqom5XjWiHpIL9TqKQ7DpRVbQNTtRtBFIKZ0iQaBk=")
            .WithHash256("lyolOjFEpwMenK+1PNbcwjIW7ZjHzw+EN8xe4louCcE=")
            .WithHash256("Tui7QoFlnLXkJCSl1/JvEZdIXTmBttnWNxzJpXomQjg=")
            .WithHash256("HmCUK6tSuR+K9jivpONI9hAkeIeDysc9CTJlS6tLklo=")
            .WithHashTagHelper()
            .ReportSample();
        builder.AddStyleSrc()
            .Self()
            .From(new[]
            {
                "https://trunk.georgia.gov/modules/custom/ga_legacy_header_footer/css/ga_legacy_footer.css",
                "https://trunk.georgia.gov/modules/custom/ga_legacy_header_footer/css/ga_legacy_header.css",
            })
            .WithHash256("wkAU1AW/h8YFx0XlzvpTllAKnFEO2tw8aKErs5a26LY=")
            .WithHash256("lyolOjFEpwMenK+1PNbcwjIW7ZjHzw+EN8xe4louCcE=")
            .ReportSample();
        builder.AddImgSrc().Self().Data().From("https://trunk.georgia.gov");
        builder.AddConnectSrc().Self().From("https://api.raygun.io");
        builder.AddFontSrc().Self();
        builder.AddFormAction().Self();
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();
        builder.AddReportUri()
            .To($"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}");
        builder.AddCustomDirective("report-to", "raygun");
    }
#pragma warning restore S1075
}
