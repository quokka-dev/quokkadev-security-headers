using FluentAssertions;
using Microsoft.Extensions.Configuration;
using QuokkaDev.SecurityHeaders.Csp;
using Xunit;

namespace QuokkaDev.SecurityHeaders.Tests
{
    public class ContentSecurityPolicyBuilderUnitTest
    {
        public ContentSecurityPolicyBuilderUnitTest()
        {
        }

        [Fact(DisplayName = "Default settings CSP should match expected string")]
        public void Default_Settings_CSP_Should_Match_Expected_String()
        {
            // Arrange
            var settings = ApplicationBuilderExtensions.GetSettingsFromDelegate(null);

            // Act            
            string csp = settings.ContentSecurityPolicy!.GetPolicyString();
            // Assert
            csp.Should().Be("default-src 'self'; object-src 'none'; child-src 'self'; frame-ancestors 'none'; upgrade-insecure-requests; block-all-mixed-content");
        }

        [Fact(DisplayName = "CSP read from config should match expected string")]
        public void CSP_Read_From_Config_Should_Match_Expected_String()
        {
            // Arrange
            ConfigurationBuilder builder = new();
            var configuration = builder.AddJsonFile("appsettings.json").Build();

            var settings = ApplicationBuilderExtensions.GetSettingsFromConfiguration(configuration, "SecurityHeaders");

            // Act            
            string csp = settings.ContentSecurityPolicy!.GetPolicyString();
            // Assert
            csp.Should().Contain("default-src 'self';");
            csp.Should().Contain("object-src 'none';");
            csp.Should().Contain("block-all-mixed-content;");
            csp.Should().NotContain("upgrade-insecure-requests;");
        }

        [Fact(DisplayName = "Helper methods should works as expected")]
        public void Helper_Methods_Should_Works_As_Expected()
        {
            // Arrange
            ContentSecurityPolicyBuilder pb = ContentSecurityPolicyBuilder.New();
            pb.AddBlockMixedContent(dir => dir.AddSource("self"));
            pb.AddChildSrc(dir => dir.AddSource("self"));
            pb.AddConnectSrc(dir => dir.AddSource("self"));
            pb.AddDefaultSrc(dir => dir.AddSource("self"));
            pb.AddFontSrc(dir => dir.AddSource("self"));
            pb.AddFrameAncestors(dir => dir.AddSource("self"));
            pb.AddFrameSrc(dir => dir.AddSource("self"));
            pb.AddImgSrc(dir => dir.AddSource("self"));
            pb.AddManifestSrc(dir => dir.AddSource("self"));
            pb.AddMediaSrc(dir => dir.AddSource("self"));
            pb.AddObjectSrc(dir => dir.AddSource("self"));
            pb.AddPrefetchSrc(dir => dir.AddSource("self"));
            pb.AddScriptAttrSrc(dir => dir.AddSource("self"));
            pb.AddScriptElemSrc(dir => dir.AddSource("self"));
            pb.AddScriptSrc(dir => dir.AddSource("self"));
            pb.AddStyleAttrSrc(dir => dir.AddSource("self"));
            pb.AddStyleElemSrc(dir => dir.AddSource("self"));
            pb.AddStyleSrc(dir => dir.AddSource("self"));
            pb.AddUpgradeInsecureRequest(dir => dir.AddSource("self"));
            pb.AddWorkeSrc(dir => dir.AddSource("self"));
            // Act
            string policy = pb.Build().GetPolicyString();

            // Assert
            policy.Should().Contain("default-src", because: "'default-src' value should be present");
            policy.Should().Contain("child-src", because: "'child-src' value should be present");
            policy.Should().Contain("connect-src", because: "'connect-src' value should be present");
            policy.Should().Contain("font-src", because: "'font-src' value should be present");
            policy.Should().Contain("frame-src", because: "'frame-src' value should be present");
            policy.Should().Contain("frame-ancestors", because: "'frame-ancestors' value should be present");
            policy.Should().Contain("img-src", because: "'img-src' value should be present");
            policy.Should().Contain("manifest-src", because: "'manifest-src' value should be present");
            policy.Should().Contain("media-src", because: "'media-src' value should be present");
            policy.Should().Contain("object-src", because: "'object-src' value should be present");
            policy.Should().Contain("prefetch-src", because: "'prefetch-src' value should be present");
            policy.Should().Contain("script-src", because: "'script-src' value should be present");
            policy.Should().Contain("script-src-elem", because: "'script-src-elem' value should be present");
            policy.Should().Contain("script-src-attr", because: "'script-src-attr' value should be present");
            policy.Should().Contain("style-src", because: "'style-src' value should be present");
            policy.Should().Contain("style-src-elem", because: "'style-src-elem' value should be present");
            policy.Should().Contain("style-src-attr", because: "'style-src-attr' value should be present");
            policy.Should().Contain("worker-src", because: "'worker-src' value should be present");
            policy.Should().Contain("upgrade-insecure-requests", because: "'upgrade-insecure-requests' value should be present");
            policy.Should().Contain("block-all-mixed-content", because: "'block-all-mixed-content' value should be present");
        }
    }
}
