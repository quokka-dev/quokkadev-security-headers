using FluentAssertions;
using Microsoft.Extensions.Configuration;
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
    }
}
