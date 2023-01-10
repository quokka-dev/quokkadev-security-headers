[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=quokka-dev_quokkadev-security-headers&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=quokka-dev_quokkadev-security-headers) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=quokka-dev_quokkadev-security-headers&metric=coverage)](https://sonarcloud.io/summary/new_code?id=quokka-dev_quokkadev-security-headers) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=quokka-dev_quokkadev-security-headers&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=quokka-dev_quokkadev-security-headers) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=quokka-dev_quokkadev-security-headers&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=quokka-dev_quokkadev-security-headers) [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=quokka-dev_quokkadev-security-headers&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=quokka-dev_quokkadev-security-headers) ![publish workflow](https://github.com/quokka-dev/quokkadev-security-headers/actions/workflows/publish.yml/badge.svg)

# QuokkaDev.SecurityHeaders
QuokkaDev.SecurityHeaders is a .NET middleware for adding OWASP security headers to your web applications. You can use online tools like [https://securityheaders.com](https://securityheaders.com) to check the status of your application against security headers.

## Installing QuokkaDev.SecurityHeaders

You should install the package via the .NET command line interface

    Install-Package QuokkaDev.SecurityHeaders

## Using QuokkaDev.SecurityHeaders
Register the middleware using the extension methods. Call the extension method in the early phase of the pipeline so the security headers will be applied to all responses.

Please note that the middleware apply some default values for headers; if you don't want some headers you must explicitly overrides the values. You can override values programmatically or reading from configuration.

#### **`startup.cs`**
```csharp

//Use headers with default values.
app.UseSecurityHeaders();

//Configure headers programmatically
app.UseSecurityHeaders(settings =>
{
    settings.XFrameOption = XFrameOption.deny;
    settings.XContentTypeOptions = XContentTypeOptions.nosniff;
    settings.UseContentSecurityPolicy = true;
    settings.UsePermissionPolicy = true;
    settings.XPermittedCrossDomainPolicies = XPermittedCrossDomainPolicies.none;
    settings.ReferrerPolicy = ReferrerPolicy.no_referrer;
    settings.CrossOriginEmbedderPolicy = CrossOriginEmbedderPolicy.require_corp;
    settings.CrossOriginOpenerPolicy = CrossOriginOpenerPolicy.same_origin;
    settings.CrossOriginResourcePolicy = CrossOriginResourcePolicy.same_origin;
    settings.ClearSiteData = new QuokkaDev.SecurityHeaders.ClearSitedata.ClearSiteData()
        .ClearCache()
        .ClearCookies()
        .ClearStorage();
    settings.ContentSecurityPolicy = ContentSecurityPolicyBuilder
        .New()
        .AddDefaultSrc( directive =>
        {
            directive.Self();
            directive.AddSource("https://my.custom.site");
        })
        .AddStyleSrc(directive =>
        {
            directive.Self();
            directive.UnsafeInline();
        })
        .Build();
    settings.PermissionPolicy = PermissionPolicyBuilder
        .New()
        .AddAccelerometer(directive => {
            directive.Self();
        })
        .Build();
});

//Use headers reading from values from configuration in a section named "SecurityHeaders"
app.UseSecurityHeaders(this.Configuration);

//You can also use a custom section name
app.UseSecurityHeaders(this.Configuration, "CustomSectionName");

//You can also read from configuration and programmatically override some values
app.UseSecurityHeaders(this.Configuration, "CustomSectionName", settings =>
{
    settings.XFrameOption = XFrameOption.none;
});
```

## Available headers

### [X-Frame-Options](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options)

Admitted values:
-   no_header (don't use header)
-   none (don't use header, deprecated)
-   deny (default value)
-   sameorigin

```csharp
app.UseSecurityHeaders(settings =>
{
    settings.XFrameOption = XFrameOption.sameorigin;
});
```

```json
{
    "SecurityHeaders": {
        "XFrameOption": "sameorigin"
    }
}
```

### [X-Content-Type-Options](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options)
Admitted values:
-   no_header (don't use header)
-   none (don't use header, deprecated)
-   nosniff (default value)

```csharp
app.UseSecurityHeaders(settings =>
{
    settings.XContentTypeOptions = XContentTypeOptions.nosniff;
});
```

```json
{
    "SecurityHeaders": {
        "XContentTypeOptions": "nosniff"
    }
}
```

### [X-Permitted-Cross-Domain-Policies](https://docs.fluidattacks.com/criteria/vulnerabilities/137/)
Admitted values:
-   no_header (don't use header)
-   none (default value)
-   master_only
-   by_content_type
-   by_ftp_filename
-   all

```csharp
app.UseSecurityHeaders(settings =>
{
    settings.XPermittedCrossDomainPolicies = XPermittedCrossDomainPolicies.none;
});
```

```json
{
    "SecurityHeaders": {
        "XPermittedCrossDomainPolicies": "none"
    }
}
```

### [Referrer-Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy)
Admitted values:
-   no_header (don't use header)
-   none (don't use header, deprecated)
-   no_referrer (default value)
-   no_referrer_when_downgrade
-   origin
-   origin_when_cross_origin
-   same_origin
-   strict_origin
-   strict_origin_when_cross_origin
-   unsafe_url


```csharp
app.UseSecurityHeaders(settings =>
{
    settings.ReferrerPolicy = ReferrerPolicy.no_referrer;
});
```

```json
{
    "SecurityHeaders": {
        "ReferrerPolicy": "no_referrer"
    }
}
```

### [Cross-Origin-Embedder-Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cross-Origin-Embedder-Policy)
Admitted values:
-   no_header (don't use header)
-   none (don't use header, deprecated)
-   unsafe_none
-   require_corp (default value)

```csharp
app.UseSecurityHeaders(settings =>
{
    settings.CrossOriginEmbedderPolicy = CrossOriginEmbedderPolicy.require_corp;
});
```

```json
{
    "SecurityHeaders": {
        "CrossOriginEmbedderPolicy": "require_corp"
    }
}
```

### [Cross-Origin-Opener-Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cross-Origin-Opener-Policy)
Admitted values:
-   no_header (don't use header)
-   none (don't use header, deprecated)
-   unsafe_none
-   same_origin_allow_popups
-   same_origin (default value)

```csharp
app.UseSecurityHeaders(settings =>
{
    settings.CrossOriginOpenerPolicy = CrossOriginOpenerPolicy.same_origin;
});
```

```json
{
    "SecurityHeaders": {
        "CrossOriginOpenerPolicy": "same_origin"
    }
}
```

### [Cross-Origin-Resource-Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cross-Origin-Resource-Policy)
Admitted values:
-   no_header (don't use header)
-   none (don't use header, deprecated)
-   same_site
-   same_origin (default value)
-   cross_origin

```csharp
app.UseSecurityHeaders(settings =>
{
    settings.CrossOriginResourcePolicy = CrossOriginResourcePolicy.same_origin;
});
```

```json
{
    "SecurityHeaders": {
        "CrossOriginResourcePolicy": "same_origin"
    }
}
```

### [Clear-Site-Data](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Clear-Site-Data)
You can configure Clear-Site-Data passing a configured ClearSitedata object. You can clear site data for **cache**, **storage** and **cookies**.
The default value for the header is

    Clear-Site-Data: "cache", "cookies", "storage"

```csharp
app.UseSecurityHeaders(settings =>
{
    settings.UseClearSiteData = true;
    settings.ClearSiteData = new ClearSitedata.ClearSiteData()
        .ClearCache()
        .ClearCookies()
        .ClearStorage();
});
```

```json
{
    "SecurityHeaders": {
        "UseClearSiteData": true, //set to false for disable header
        "ClearSiteData": [ "cache", "cookies" , "storage"]
    }
}
```

### [Content-Security-Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy)
You can configure Content-Security-Policy passing a configured ContentSecurityPolicy object. You can configure the object using configuration, using a "ready-to-use" string or using a `ContentSecurityPolicyBuilder` with fluent API.
The default value for the header is:

    Content-Security-Policy: default-src 'self'; object-src 'none'; child-src 'self'; frame-ancestors 'none'; upgrade-insecure-requests; block-all-mixed-content

```csharp
//Configure CSP with a string
app.UseSecurityHeaders(settings =>
{
    settings.ContentSecurityPolicy = ContentSecurityPolicyBuilder
    .New("default-src 'self'; object-src 'none'")
    .Build();
});

//Configure CSP with fluent API
app.UseSecurityHeaders(settings =>
{
    settings.ContentSecurityPolicy = ContentSecurityPolicyBuilder
    .New()
    .AddDefaultSrc(directives => {
        directives.Self();
        directives.UnsafeInline();
        directives.AddSource("https://github.com");
    })
    .Build();
});

//Disable CSP header
app.UseSecurityHeaders(settings =>
{
    settings.UseContentSecurityPolicy = false;
});
```
If both a string and fluent API are used with `ContentSecurityPolicyBuilder` the string take the precedence

```json
{
    "SecurityHeaders": {
        "UseContentSecurityPolicy": true //set to false for disable header
        "ContentSecurityPolicy": {
            "default-src": [ "'self'" ],
            "object-src": [ "'none'" ],
            "block-all-mixed-content": [ "" ],
            "child-src": [ "'self'" ],
            "frame-ancestors": [ "'none'" ],
            "upgrade-insecure-requests": [ "" ],
            "style-src": [ "'self'", "https://fonts.googleapis.com", "'nonce'" ],
            "font-src": [ "'self'", "https://fonts.gstatic.com" ],
            "script-src": [ "'self'", "'nonce'" ],
            "script-src-elem": [ "'self'", "'nonce'" ],
            "connect-src": [ "'self'" ],
            "prefetch-src": [ "'self'", "https://fonts.googleapis.com", "'nonce'" ]
        }
    }
}
```
Configure a directive calling the right method on the builder then adding all the allowed sources with `AddSource()`. Some utility methods like `All(), None(), Self(), UnsafeInline(), UnsafeEval()` can be used for standard values

If a directive take no extra values (like *block-all-mixed-content*) pass an array with an empty string in configuration file.

#### Ignore Urls
In some cases it can be useful disable the Content Security Policy for some URLs in your application. For example, if you use  NuGet package it serve an index.html page with inline styles and scripts. You can bypass the problem using 'unsafe-inline' as a CSP value for styles and script but this makes the whole application much more insecure. You can disable CSP for the swagger endpoint using the property `ContentSecurityPolicyIgnoreUrls` and passing an array of path to ignore:

```csharp
app.UseSecurityHeaders(settings =>
{
    settings.ContentSecurityPolicyIgnoreUrls = new string[] { "/swagger/index.html" };
});
```

```json
{
    "SecurityHeaders": {
        "ContentSecurityPolicyIgnoreUrls": [ "/swagger/index.html" ]
    }
}
```

#### Nonce
If you need to use *nonce* for inline script and styles call `AddSource("'nonce'")` in fluent API or use the string `"'nonce'"` in configuration file.

Using *nonce* require that you configure a service for nonce generation in your startup file, so call the extension method:

```csharp
services.AddNonceService();
```

The service inject a different nonce value in the header foreach request.
Using this service you can easily build a TagHelper for RazorView pages:

```csharp

```

```csharp
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using QuokkaDev.SecurityHeaders;

namespace MyProject.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = "add-nonce")]
    public class NonceTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public NonceTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("add-nonce");
            output.Attributes.Add(new TagHelperAttribute("nonce", new HtmlString(_contextAccessor.GetNonce())));
        }
    }
}
```

```html
<script type="text/javascript" add-nonce>
    alert('now inline script works!!!');
</script>
```


### [Permissions-Policy](https://w3c.github.io/webappsec-permissions-policy)
You can configure Permissions-Policy passing a configured PermissionPolicy object. You can configure the object using configuration, using a "ready-to-use" string or using a `PermissionPolicyBuilder` with fluent API.
The default value for the header is:

    Permissions-Policy: accelerometer=(),autoplay=(),camera=(),display-capture=(),document-domain=(),encrypted-media=(),fullscreen=(),geolocation=(),gyroscope=(),magnetometer=(),microphone=(),midi=(),payment=(),picture-in-picture=(),publickey-credentials-get=(),screen-wake-lock=(),sync-xhr=(self),usb=(),web-share=(),xr-spatial-tracking=()

```csharp
//Configure permissions policy with a string
app.UseSecurityHeaders(settings =>
{
    settings.PermissionPolicy = PermissionPolicyBuilder
    .New("accelerometer=(),autoplay=(),camera=()")
    .Build();
});

//Configure permissions policy with fluent API
app.UseSecurityHeaders(settings =>
{
    settings.PermissionPolicy = PermissionPolicyBuilder
        .New()
        .AddCamera(directives => {
            directives.Self();
        })
        .Build();
});

//Disable permissions policy header
app.UseSecurityHeaders(settings =>
{
    settings.UsePermissionPolicy = false;
});
```
If both a string and fluent API are used with `PermissionsPolicyBuilder` the string take the precedence

```json
{
    "SecurityHeaders": {
        "UsePermissionPolicy": true //set to false for disable header
        "PermissionPolicy": {
            "accelerometer": [""],
            "camera": [ "self" ],
            "display-capture": ["self", "https://mydomain"]
        }
    }
}
```

Configure a directive calling the right method on the builder then adding all the allowed sources with `AddSource()`. Utility method like `Self()` can be used for *'self'* standard value

If a directive take no extra values pass an array with an empty string in configuration file.

