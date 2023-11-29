using Microsoft.Extensions.Configuration;

namespace QuokkaDev.SecurityHeaders.PermissionPolicy
{
    public sealed class PermissionPolicyBuilder
    {
        private readonly PermissionPolicy policy;

        private PermissionPolicyBuilder()
        {
            this.policy = new PermissionPolicy();
        }

        private PermissionPolicyBuilder(string policyString)
        {
            this.policy = new PermissionPolicy(policyString);
        }

        public static PermissionPolicyBuilder New()
        {
            return new PermissionPolicyBuilder();
        }

        public static PermissionPolicyBuilder New(string policyString)
        {
            return new PermissionPolicyBuilder(policyString);
        }

        public PermissionPolicyBuilder ReadFromConfig(IConfiguration config, string sectionName)
        {
            var keys = config.GetSection(sectionName).GetChildren();
            if (keys != null)
            {
                foreach (string key in keys.Select(k => k.Key))
                {
                    var allowedSources = config.GetSection(sectionName).GetSection(key).Get<string[]>()?.ToList();
                    AddDirective(key, allowedSources);
                }
            }
            return this;
        }

        public PermissionPolicyBuilder AddDirective(string name, List<string>? allowedSources)
        {
            return AddDirective(name, p =>
            {
                allowedSources ??= new List<string>();
                foreach (string source in allowedSources)
                {
                    p.AddSource(source);
                }
            });
        }

        public PermissionPolicyBuilder AddAccelerometer(Action<Directive> configureDirective)
        {
            return AddDirective("accelerometer", configureDirective);
        }

        public PermissionPolicyBuilder AddAmbientLightSensor(Action<Directive> configureDirective)
        {
            return AddDirective("ambient-light-sensor", configureDirective);
        }

        public PermissionPolicyBuilder AddAutoplay(Action<Directive> configureDirective)
        {
            return AddDirective("autoplay", configureDirective);
        }

        public PermissionPolicyBuilder AddBattery(Action<Directive> configureDirective)
        {
            return AddDirective("battery", configureDirective);
        }

        public PermissionPolicyBuilder AddCamera(Action<Directive> configureDirective)
        {
            return AddDirective("camera", configureDirective);
        }

        public PermissionPolicyBuilder AddDisplayCapture(Action<Directive> configureDirective)
        {
            return AddDirective("display-capture", configureDirective);
        }

        public PermissionPolicyBuilder AddDocumentDomain(Action<Directive> configureDirective)
        {
            return AddDirective("document-domain", configureDirective);
        }

        public PermissionPolicyBuilder AddEncryptedMedia(Action<Directive> configureDirective)
        {
            return AddDirective("encrypted-media", configureDirective);
        }

        public PermissionPolicyBuilder AddExecutionWhileNotRendered(Action<Directive> configureDirective)
        {
            return AddDirective("execution-while-not-rendered", configureDirective);
        }

        public PermissionPolicyBuilder AddExecutionWhileOutOfViewport(Action<Directive> configureDirective)
        {
            return AddDirective("execution-while-out-of-viewport", configureDirective);
        }

        public PermissionPolicyBuilder AddFullscreen(Action<Directive> configureDirective)
        {
            return AddDirective("fullscreen", configureDirective);
        }

        public PermissionPolicyBuilder AddGeolocation(Action<Directive> configureDirective)
        {
            return AddDirective("geolocation", configureDirective);
        }

        public PermissionPolicyBuilder AddGyroscope(Action<Directive> configureDirective)
        {
            return AddDirective("gyroscope", configureDirective);
        }

        public PermissionPolicyBuilder AddLayoutAnimations(Action<Directive> configureDirective)
        {
            return AddDirective("layout-animations", configureDirective);
        }

        public PermissionPolicyBuilder AddLegacyImageFormats(Action<Directive> configureDirective)
        {
            return AddDirective("legacy-image-formats", configureDirective);
        }

        public PermissionPolicyBuilder AddMagnetometer(Action<Directive> configureDirective)
        {
            return AddDirective("magnetometer", configureDirective);
        }

        public PermissionPolicyBuilder AddMicrophone(Action<Directive> configureDirective)
        {
            return AddDirective("microphone", configureDirective);
        }

        public PermissionPolicyBuilder AddMidi(Action<Directive> configureDirective)
        {
            return AddDirective("midi", configureDirective);
        }

        public PermissionPolicyBuilder AddNavigationOverride(Action<Directive> configureDirective)
        {
            return AddDirective("navigation-override", configureDirective);
        }

        public PermissionPolicyBuilder AddOversizedImages(Action<Directive> configureDirective)
        {
            return AddDirective("oversized-images", configureDirective);
        }

        public PermissionPolicyBuilder AddPayment(Action<Directive> configureDirective)
        {
            return AddDirective("payment", configureDirective);
        }

        public PermissionPolicyBuilder AddPictureInPicture(Action<Directive> configureDirective)
        {
            return AddDirective("picture-in-picture", configureDirective);
        }

        public PermissionPolicyBuilder AddPublicKeyCredentialsGet(Action<Directive> configureDirective)
        {
            return AddDirective("publickey-credentials-get", configureDirective);
        }

        public PermissionPolicyBuilder AddSyncXhr(Action<Directive> configureDirective)
        {
            return AddDirective("sync-xhr", configureDirective);
        }

        public PermissionPolicyBuilder AddUsb(Action<Directive> configureDirective)
        {
            return AddDirective("usb", configureDirective);
        }

        public PermissionPolicyBuilder AddVr(Action<Directive> configureDirective)
        {
            return AddDirective("vr", configureDirective);
        }

        public PermissionPolicyBuilder AddWakeLock(Action<Directive> configureDirective)
        {
            return AddDirective("wake-lock", configureDirective);
        }

        public PermissionPolicyBuilder AddScreenWakeLock(Action<Directive> configureDirective)
        {
            return AddDirective("screen-wake-lock", configureDirective);
        }

        public PermissionPolicyBuilder AddWebShare(Action<Directive> configureDirective)
        {
            return AddDirective("web-share", configureDirective);
        }

        public PermissionPolicyBuilder AddXrSpatialTracking(Action<Directive> configureDirective)
        {
            return AddDirective("xr-spatial-tracking", configureDirective);
        }

        public PermissionPolicy Build()
        {
            return policy;
        }

        private PermissionPolicyBuilder AddDirective(string name, Action<Directive>? configureDirective = null)
        {
            configureDirective?.Invoke(policy.Add(name, new Directive() { Name = name }));
            return this;
        }
    }
}
