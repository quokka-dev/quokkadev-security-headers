namespace QuokkaDev.SecurityHeaders
{
    internal static class Constants
    {
        internal static class Headers
        {
            public const string X_FRAME_OPTIONS = "X-Frame-Options";
            public const string X_CONTENT_TYPE_OPTIONS = "X-Content-Type-Options";
            public const string CONTENT_SECURITY_POLICY = "Content-Security-Policy";
            public const string X_PERMITTED_CROSS_DOMAIN_POLICIES = "X-Permitted-Cross-Domain-Policies";
            public const string REFERRER_POLICY = "Referrer-Policy";
            public const string CLEAR_SITE_DATA = "Clear-Site-Data";
            public const string CROSS_ORIGIN_EMBEDDER_POLICY = "Cross-Origin-Embedder-Policy";
            public const string CROSS_ORIGIN_OPENER_POLICY = "Cross-Origin-Opener-Policy";
            public const string CROSS_ORIGIN_RESOURCE_POLICY = "Cross-Origin-Resource-Policy";
            public const string PERMISSION_POLICY = "Permissions-Policy";
        }
    }

    public enum XFrameOption
    {
        none, //No headers
        deny,
        sameorigin
    }

    public enum XContentTypeOptions
    {
        none,
        nosniff
    }

    public enum XPermittedCrossDomainPolicies
    {
        no_header, //if you want no header added
        none, //header added with value 'none'
        master_only,
        by_content_type,
        by_ftp_filename,
        all
    }

    public enum ReferrerPolicy
    {
        none,
        no_referrer,
        no_referrer_when_downgrade,
        origin,
        origin_when_cross_origin,
        same_origin,
        strict_origin,
        strict_origin_when_cross_origin,
        unsafe_url
    }

    public enum CrossOriginEmbedderPolicy
    {
        none,
        unsafe_none,
        require_corp
    }

    public enum CrossOriginOpenerPolicy
    {
        none,
        unsafe_none,
        same_origin_allow_popups,
        same_origin
    }

    public enum CrossOriginResourcePolicy
    {
        none,
        same_site,
        same_origin,
        cross_origin
    }
}
