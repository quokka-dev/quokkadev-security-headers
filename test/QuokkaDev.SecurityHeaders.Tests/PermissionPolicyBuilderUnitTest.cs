using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace QuokkaDev.SecurityHeaders.Tests
{
    public class PermissionPolicyBuilderUnitTest
    {
        public PermissionPolicyBuilderUnitTest()
        {
        }

        [Fact(DisplayName = "Default settings Permission Policy should match expected string")]
        public void Default_Settings_CSP_Should_Match_Expected_String()
        {
            // Arrange
            var settings = ApplicationBuilderExtensions.GetSettingsFromDelegate(null);

            // Act            
            string permissionPolicy = settings.PermissionPolicy!.GetPolicyString();
            // Assert
            permissionPolicy.Should().Be("accelerometer=(),autoplay=(),camera=(),display-capture=(),document-domain=(),encrypted-media=(),fullscreen=(),geolocation=(),gyroscope=(),magnetometer=(),microphone=(),midi=(),payment=(),picture-in-picture=(),publickey-credentials-get=(),screen-wake-lock=(),sync-xhr=(self),usb=(),web-share=(),xr-spatial-tracking=()");
        }

        [Fact(DisplayName = "PermissionPolicy read from config should match expected string")]
        public void CSP_Read_From_Config_Should_Match_Expected_String()
        {
            // Arrange
            ConfigurationBuilder builder = new ConfigurationBuilder();
            var configuration = builder.AddJsonFile("appsettings.json").Build();

            var settings = ApplicationBuilderExtensions.GetSettingsFromConfiguration(configuration, "SecurityHeaders");

            // Act            
            string permissionPolicy = settings.PermissionPolicy!.GetPolicyString();
            // Assert
            permissionPolicy.Should().Contain("accelerometer=()");
            permissionPolicy.Should().Contain("camera=(self)");
            permissionPolicy.Should().Contain("display-capture=(self \"https://mydomain\")");
            permissionPolicy.Should().NotContain("gyroscope");
        }
    }
}
