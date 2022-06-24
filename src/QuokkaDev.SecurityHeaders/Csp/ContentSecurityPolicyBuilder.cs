using Microsoft.Extensions.Configuration;

namespace QuokkaDev.SecurityHeaders.Csp
{
    public class ContentSecurityPolicyBuilder
    {
        private readonly ContentSecurityPolicy policy;

        private ContentSecurityPolicyBuilder()
        {
            this.policy = new ContentSecurityPolicy();
        }

        private ContentSecurityPolicyBuilder(string policyString)
        {
            this.policy = new ContentSecurityPolicy(policyString);
        }

        public static ContentSecurityPolicyBuilder New()
        {
            return new ContentSecurityPolicyBuilder();
        }

        public static ContentSecurityPolicyBuilder New(string policyString)
        {
            return new ContentSecurityPolicyBuilder(policyString);
        }

        public ContentSecurityPolicyBuilder ReadFromConfig(IConfiguration config, string sectionName)
        {
            var keys = config?.GetSection(sectionName)?.GetChildren();
            if (keys != null)
            {
                foreach (string key in keys.Select(k => k.Key))
                {
                    var allowedSources = config?.GetSection(sectionName).GetSection(key).Get<string[]>().ToList();
                    AddDirective(key, allowedSources);
                }
            }
            return this;
        }

        public ContentSecurityPolicyBuilder AddDirective(string name, List<string>? allowedSources)
        {
            return AddDirective(name, p =>
            {
                if (allowedSources != null)
                {
                    foreach (string source in allowedSources)
                    {
                        p.AddSource(source);
                    }
                }
            });
        }

        public ContentSecurityPolicyBuilder AddDefaultSrc(Action<Directive> configureDirective)
        {
            return AddDirective("default-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddChildSrc(Action<Directive> configureDirective)
        {
            return AddDirective("child-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddConnectSrc(Action<Directive> configureDirective)
        {
            return AddDirective("connect-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddFontSrc(Action<Directive> configureDirective)
        {
            return AddDirective("font-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddFrameSrc(Action<Directive> configureDirective)
        {
            return AddDirective("frame-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddFrameAncestors(Action<Directive> configureDirective)
        {
            return AddDirective("frame-ancestors", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddImgSrc(Action<Directive> configureDirective)
        {
            return AddDirective("img-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddManifestSrc(Action<Directive> configureDirective)
        {
            return AddDirective("manifest-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddMediaSrc(Action<Directive> configureDirective)
        {
            return AddDirective("media-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddObjectSrc(Action<Directive> configureDirective)
        {
            return AddDirective("object-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddPrefetchSrc(Action<Directive> configureDirective)
        {
            return AddDirective("prefetch-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddScriptSrc(Action<Directive> configureDirective)
        {
            return AddDirective("script-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddScriptElemSrc(Action<Directive> configureDirective)
        {
            return AddDirective("script-src-elem", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddScriptAttrSrc(Action<Directive> configureDirective)
        {
            return AddDirective("script-src-attr", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddStyleSrc(Action<Directive> configureDirective)
        {
            return AddDirective("style-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddStyleElemSrc(Action<Directive> configureDirective)
        {
            return AddDirective("style-src-elem", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddStyleAttrSrc(Action<Directive> configureDirective)
        {
            return AddDirective("style-src-attr", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddWorkeSrc(Action<Directive> configureDirective)
        {
            return AddDirective("worker-src", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddUpgradeInsecureRequest(Action<Directive> configureDirective)
        {
            return AddDirective("upgrade-insecure-requests", configureDirective);
        }

        public ContentSecurityPolicyBuilder AddBlockMixedContent(Action<Directive> configureDirective)
        {
            return AddDirective("block-all-mixed-content", configureDirective);
        }

        public ContentSecurityPolicy Build()
        {
            return policy;
        }

        private ContentSecurityPolicyBuilder AddDirective(string name, Action<Directive>? configureDirective = null)
        {
            configureDirective?.Invoke(policy.Add(name, new Directive() { Name = name }));
            return this;
        }
    }
}
