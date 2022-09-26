using FluentAssertions;
using Microsoft.Extensions.Configuration;
using QuokkaDev.SecurityHeaders.PermissionPolicy;
using System.Collections.Generic;
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
            ConfigurationBuilder builder = new();
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



        [Fact(DisplayName = "Using array of sources should works as expected")]
        public void Using_Array_Of_Sources_Should_Works_As_Expected()
        {
            // Arrange
            PermissionPolicyBuilder pb = PermissionPolicyBuilder.New();
            pb.AddDirective("MyDirective", new List<string>() { "self", "http://localhost" });

            // Act
            string policy = pb.Build().GetPolicyString();

            // Assert            
            policy.Should().Contain("self", because: "'self' value should be present");
            policy.Should().Contain("http://localhost", because: "'http://localhost' value should be present");
        }

        [Fact(DisplayName = "Helper methods should works as expected")]
        public void Helper_Methods_Should_Works_As_Expected()
        {
            // Arrange
            PermissionPolicyBuilder pb = PermissionPolicyBuilder.New();
            pb.AddAccelerometer(dir => dir.AddSource("self"));
            pb.AddAmbientLightSensor(dir => dir.AddSource("self"));
            pb.AddAutoplay(dir => dir.AddSource("self"));
            pb.AddBattery(dir => dir.AddSource("self"));
            pb.AddCamera(dir => dir.AddSource("self"));
            pb.AddDisplayCapture(dir => dir.AddSource("self"));
            pb.AddDocumentDomain(dir => dir.AddSource("self"));
            pb.AddEncryptedMedia(dir => dir.AddSource("self"));
            pb.AddExecutionWhileNotRendered(dir => dir.AddSource("self"));
            pb.AddExecutionWhileOutOfViewport(dir => dir.AddSource("self"));
            pb.AddFullscreen(dir => dir.AddSource("self"));
            pb.AddGeolocation(dir => dir.AddSource("self"));
            pb.AddGyroscope(dir => dir.AddSource("self"));
            pb.AddLayoutAnimations(dir => dir.AddSource("self"));
            pb.AddLegacyImageFormats(dir => dir.AddSource("self"));
            pb.AddMagnetometer(dir => dir.AddSource("self"));
            pb.AddMicrophone(dir => dir.AddSource("self"));
            pb.AddMidi(dir => dir.AddSource("self"));
            pb.AddNavigationOverride(dir => dir.AddSource("self"));
            pb.AddOversizedImages(dir => dir.AddSource("self"));
            pb.AddPayment(dir => dir.AddSource("self"));
            pb.AddPictureInPicture(dir => dir.AddSource("self"));
            pb.AddPublicKeyCredentialsGet(dir => dir.AddSource("self"));
            pb.AddScreenWakeLock(dir => dir.AddSource("self"));
            pb.AddSyncXhr(dir => dir.AddSource("self"));
            pb.AddUsb(dir => dir.AddSource("self"));
            pb.AddVr(dir => dir.AddSource("self"));
            pb.AddWakeLock(dir => dir.AddSource("self"));
            pb.AddWebShare(dir => dir.AddSource("self"));
            pb.AddXrSpatialTracking(dir => dir.AddSource("self"));

            // Act
            string policy = pb.Build().GetPolicyString();

            // Assert
            policy.Should().Contain("accelerometer", because: "'accelerometer' value should be present");
            policy.Should().Contain("ambient-light-sensor", because: "'ambient-light-sensor' value should be present");
            policy.Should().Contain("autoplay", because: "'autoplay' value should be present");
            policy.Should().Contain("battery", because: "'battery' value should be present");
            policy.Should().Contain("camera", because: "'camera' value should be present");
            policy.Should().Contain("display-capture", because: "'display-capture' value should be present");
            policy.Should().Contain("document-domain", because: "'document-domain' value should be present");
            policy.Should().Contain("encrypted-media", because: "'encrypted-media' value should be present");
            policy.Should().Contain("execution-while-not-rendered", because: "'execution-while-not-rendered' value should be present");
            policy.Should().Contain("execution-while-out-of-viewport", because: "'execution-while-out-of-viewport' value should be present");
            policy.Should().Contain("fullscreen", because: "'fullscreen' value should be present");
            policy.Should().Contain("geolocation", because: "'geolocation' value should be present");
            policy.Should().Contain("gyroscope", because: "'gyroscope' value should be present");
            policy.Should().Contain("layout-animations", because: "'layout-animations' value should be present");
            policy.Should().Contain("legacy-image-formats", because: "'legacy-image-formats' value should be present");
            policy.Should().Contain("magnetometer", because: "'magnetometer' value should be present");
            policy.Should().Contain("microphone", because: "'microphone' value should be present");
            policy.Should().Contain("midi", because: "'midi' value should be present");
            policy.Should().Contain("navigation-override", because: "'navigation-override' value should be present");
            policy.Should().Contain("oversized-images", because: "'oversized-images' value should be present");
            policy.Should().Contain("payment", because: "'payment' value should be present");
            policy.Should().Contain("picture-in-picture", because: "'picture-in-picture' value should be present");
            policy.Should().Contain("publickey-credentials-get", because: "'publickey-credentials-get' value should be present");
            policy.Should().Contain("sync-xhr", because: "'sync-xhr' value should be present");
            policy.Should().Contain("screen-wake-lock", because: "'screen-wake-lock' value should be present");
            policy.Should().Contain("usb", because: "'usb' value should be present");
            policy.Should().Contain("vr", because: "'vr' value should be present");
            policy.Should().Contain("wake-lock", because: "'wake-lock' value should be present");
            policy.Should().Contain("web-share", because: "'web-share' value should be present");
            policy.Should().Contain("xr-spatial-tracking", because: "'xr-spatial-tracking' value should be present");
        }
    }
}
