using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuokkaDev.SecurityHeaders.Csp;

namespace QuokkaDev.SecurityHeaders
{
    /// <summary>
    /// A middleware for adding <see href="https://owasp.org/www-project-secure-headers/">OWASP security headers</see>see>
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate next;
        private readonly SecurityHeadersConfigurationSettings settings;
        private readonly ILogger<SecurityHeadersMiddleware>? logger;

        public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeadersConfigurationSettings settings, ILogger<SecurityHeadersMiddleware>? logger)
        {
            this.next = next;
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// Add headers to the HTTP response and call the next middleware
        /// </summary>
        /// <param name="httpContext">Current HTTP context</param>
        /// <returns></returns>
        public Task InvokeAsync(HttpContext httpContext)
        {
            AddXFrameOption(httpContext);
            AddXContentTypeOptions(httpContext);
            AddContentSecurityPolicy(httpContext);
            AddXPermittedCrossDomainPolicies(httpContext);
            AddReferrerPolicy(httpContext);
            AddClearSiteData(httpContext);
            AddCrossOriginEmbedderPolicy(httpContext);
            AddCrossOriginOpenerPolicy(httpContext);
            AddCrossOriginResourcePolicy(httpContext);
            AddPermissionPolicy(httpContext);
            return next(httpContext);
        }

        private void AddXFrameOption(HttpContext httpContext)
        {
            if (settings.XFrameOption != XFrameOption.none)
            {
                logger?.LogTrace("Adding XFrameOption header with value {header}", settings.XFrameOption);
                httpContext.Response.Headers.TryAdd(Constants.Headers.X_FRAME_OPTIONS, settings.XFrameOption.ToString());
            }
        }

        private void AddXContentTypeOptions(HttpContext httpContext)
        {
            if (settings.XContentTypeOptions == XContentTypeOptions.nosniff)
            {
                httpContext.Response.Headers.TryAdd(Constants.Headers.X_CONTENT_TYPE_OPTIONS, nameof(XContentTypeOptions.nosniff));
            }
        }

        private void AddContentSecurityPolicy(HttpContext httpContext)
        {
            if (settings.UseContentSecurityPolicy && settings.ContentSecurityPolicy != null)
            {
                var service = httpContext.RequestServices.GetService(typeof(INonceService));
                string contentSecuritypolicyString = settings.ContentSecurityPolicy.GetPolicyString();
                if (service is INonceService nonceService)
                {
                    contentSecuritypolicyString = contentSecuritypolicyString.Replace("'nonce'", $"'nonce-{nonceService.RequestNonce}'");
                }
                httpContext.Response.Headers.TryAdd(Constants.Headers.CONTENT_SECURITY_POLICY, contentSecuritypolicyString);
            }
        }

        private void AddPermissionPolicy(HttpContext httpContext)
        {
            if (settings.UsePermissionPolicy && settings.PermissionPolicy != null)
            {
                httpContext.Response.Headers.TryAdd(Constants.Headers.PERMISSION_POLICY, settings.PermissionPolicy.GetPolicyString());
            }
        }

        private void AddXPermittedCrossDomainPolicies(HttpContext httpContext)
        {
            if (settings.XPermittedCrossDomainPolicies != XPermittedCrossDomainPolicies.no_header)
            {
                httpContext.Response.Headers.TryAdd(Constants.Headers.X_PERMITTED_CROSS_DOMAIN_POLICIES, settings.XPermittedCrossDomainPolicies.DashReplace());
            }
        }

        private void AddReferrerPolicy(HttpContext httpContext)
        {
            if (settings.ReferrerPolicy != ReferrerPolicy.none)
            {
                httpContext.Response.Headers.TryAdd(Constants.Headers.REFERRER_POLICY, settings.ReferrerPolicy.DashReplace());
            }
        }

        private void AddClearSiteData(HttpContext httpContext)
        {
            if (settings.ClearSiteData != null)
            {
                httpContext.Response.Headers.TryAdd(Constants.Headers.CLEAR_SITE_DATA, settings.ClearSiteData.ToString());
            }
        }

        private void AddCrossOriginEmbedderPolicy(HttpContext httpContext)
        {
            if (settings.CrossOriginEmbedderPolicy != CrossOriginEmbedderPolicy.none)
            {
                httpContext.Response.Headers.TryAdd(Constants.Headers.CROSS_ORIGIN_EMBEDDER_POLICY, settings.CrossOriginEmbedderPolicy.DashReplace());
            }
        }

        private void AddCrossOriginOpenerPolicy(HttpContext httpContext)
        {
            if (settings.CrossOriginOpenerPolicy != CrossOriginOpenerPolicy.none)
            {
                httpContext.Response.Headers.TryAdd(Constants.Headers.CROSS_ORIGIN_OPENER_POLICY, settings.CrossOriginOpenerPolicy.DashReplace());
            }
        }

        private void AddCrossOriginResourcePolicy(HttpContext httpContext)
        {
            if (settings.CrossOriginResourcePolicy != CrossOriginResourcePolicy.none)
            {
                httpContext.Response.Headers.TryAdd(Constants.Headers.CROSS_ORIGIN_RESOURCE_POLICY, settings.CrossOriginResourcePolicy.DashReplace());
            }
        }
    }
}
