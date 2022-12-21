using QuokkaDev.SecurityHeaders.ClearSitedata;
using QuokkaDev.SecurityHeaders.Csp;
namespace QuokkaDev.SecurityHeaders
{
    /// <summary>
    /// Settings for configuring headers
    /// </summary>
    public class SecurityHeadersConfigurationSettings
    {
        public XFrameOption XFrameOption { get; set; }
        public XContentTypeOptions XContentTypeOptions { get; set; }
        public bool UseContentSecurityPolicy { get; set; }
        public ContentSecurityPolicy? ContentSecurityPolicy { get; set; }
        public XPermittedCrossDomainPolicies XPermittedCrossDomainPolicies { get; set; }
        public ReferrerPolicy ReferrerPolicy { get; set; }
        public ClearSiteData? ClearSiteData { get; set; }
        public CrossOriginEmbedderPolicy CrossOriginEmbedderPolicy { get; set; }
        public CrossOriginOpenerPolicy CrossOriginOpenerPolicy { get; set; }
        public CrossOriginResourcePolicy CrossOriginResourcePolicy { get; set; }
        public bool UsePermissionPolicy { get; set; }
        public PermissionPolicy.PermissionPolicy? PermissionPolicy { get; set; }
        public bool UseClearSiteData { get; set; }
        public string[]? ContentSecurityPolicyIgnoreUrls { get; set; }
    }
}
