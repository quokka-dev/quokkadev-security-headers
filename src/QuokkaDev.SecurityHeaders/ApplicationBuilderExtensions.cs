using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using QuokkaDev.SecurityHeaders.Csp;
using QuokkaDev.SecurityHeaders.PermissionPolicy;

namespace QuokkaDev.SecurityHeaders
{
    /// <summary>
    /// Extensions method for register security headers middleware
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add SecurityHeadersMiddleware to the application pipeline
        /// </summary>
        /// <param name="builder">The configuring application builder</param>
        /// <param name="configureSettingsDelegate">A delegate for settings configuration</param>
        /// <returns>The configuring application builder for chaining methods</returns>
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder, Action<SecurityHeadersConfigurationSettings>? configureSettingsDelegate = null)
        {
            var settings = GetSettingsFromDelegate(configureSettingsDelegate);
            return builder.UseMiddleware<SecurityHeadersMiddleware>(settings);
        }

        /// <summary>
        /// Add SecurityHeadersMiddleware to the application pipeline
        /// </summary>
        /// <param name="builder">The configuring application builder</param>
        /// <param name="config">Application IConfiguration</param>
        /// <param name="sectionName">The name of the configuration section where settings are read. Default is "SecurityHeaders"</param>
        /// <returns>The configuring application builder for chaining methods</returns>
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder, IConfiguration config, string sectionName = "SecurityHeaders")
        {
            var settings = GetSettingsFromConfiguration(config, sectionName);
            return builder.UseMiddleware<SecurityHeadersMiddleware>(settings);
        }

        /// <summary>
        /// Build security headers settings from a delegate action
        /// </summary>
        /// <param name="configureSettingsDelegate">The delegate for configuring settings</param>
        /// <returns>The configured settings</returns>
        internal static SecurityHeadersConfigurationSettings GetSettingsFromDelegate(Action<SecurityHeadersConfigurationSettings>? configureSettingsDelegate)
        {
            configureSettingsDelegate ??= ((SecurityHeadersConfigurationSettings _) => { });
            var settings = GetDefaultSettings();

            configureSettingsDelegate?.Invoke(settings);

            return settings;
        }

        /// <summary>
        /// Build security headers settings from IConfiguration
        /// </summary>
        /// <param name="config">Application IConfiguration</param>
        /// <param name="sectionName">The name of the configuration section where settings are read</param>
        /// <returns>The settings read from configuration</returns>
        internal static SecurityHeadersConfigurationSettings GetSettingsFromConfiguration(IConfiguration config, string sectionName)
        {
            var settings = GetDefaultSettings();
            config.Bind(sectionName, settings);
            settings.ContentSecurityPolicy = ContentSecurityPolicyBuilder.New().ReadFromConfig(config, $"{sectionName}:ContentSecurityPolicy").Build();
            settings.PermissionPolicy = PermissionPolicyBuilder.New().ReadFromConfig(config, $"{sectionName}:PermissionPolicy").Build();
            settings.ClearSiteData = ClearSitedata.ClearSiteData.ReadFromConfig(config, $"{sectionName}:ClearSiteData") ?? settings.ClearSiteData;
            return settings;
        }

        private static SecurityHeadersConfigurationSettings GetDefaultSettings()
        {
            var settings = new SecurityHeadersConfigurationSettings()
            {
                XFrameOption = XFrameOption.deny,
                XContentTypeOptions = XContentTypeOptions.nosniff,
                UseContentSecurityPolicy = true,
                UsePermissionPolicy = true,
                XPermittedCrossDomainPolicies = XPermittedCrossDomainPolicies.none,
                ReferrerPolicy = ReferrerPolicy.no_referrer,
                CrossOriginEmbedderPolicy = CrossOriginEmbedderPolicy.require_corp,
                CrossOriginOpenerPolicy = CrossOriginOpenerPolicy.same_origin,
                CrossOriginResourcePolicy = CrossOriginResourcePolicy.same_origin,
                ClearSiteData = new ClearSitedata.ClearSiteData()
                    .ClearCache()
                    .ClearCookies()
                    .ClearStorage(),
                ContentSecurityPolicy = ContentSecurityPolicyBuilder
                    .New("default-src 'self'; object-src 'none'; child-src 'self'; frame-ancestors 'none'; upgrade-insecure-requests; block-all-mixed-content")
                    .Build(),
                PermissionPolicy = PermissionPolicyBuilder
                    .New("accelerometer=(),autoplay=(),camera=(),display-capture=(),document-domain=(),encrypted-media=(),fullscreen=(),geolocation=(),gyroscope=(),magnetometer=(),microphone=(),midi=(),payment=(),picture-in-picture=(),publickey-credentials-get=(),screen-wake-lock=(),sync-xhr=(self),usb=(),web-share=(),xr-spatial-tracking=()")
                    .Build()
            };
            return settings;
        }
    }
}
