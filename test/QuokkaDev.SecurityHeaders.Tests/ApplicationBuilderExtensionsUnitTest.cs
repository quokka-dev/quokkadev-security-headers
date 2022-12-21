using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuokkaDev.SecurityHeaders.ClearSitedata;
using QuokkaDev.SecurityHeaders.Csp;
using System.Collections.Generic;
using Xunit;

namespace QuokkaDev.SecurityHeaders.Tests;

public class ApplicationBuilderExtensionsUnitTest
{
    [Fact(DisplayName = "Action delegate should configure settings correctly")]
    public void Action_Delegate_Should_Configure_Settings_Correctly()
    {
        // Arrange
        var settings = ApplicationBuilderExtensions.GetSettingsFromDelegate(opts =>
        {
            opts.XFrameOption = XFrameOption.sameorigin;
            opts.XContentTypeOptions = XContentTypeOptions.no_header;
            opts.XPermittedCrossDomainPolicies = XPermittedCrossDomainPolicies.all;
            opts.ReferrerPolicy = ReferrerPolicy.strict_origin;
            opts.ClearSiteData = new ClearSiteData("*");
            opts.CrossOriginEmbedderPolicy = CrossOriginEmbedderPolicy.unsafe_none;
            opts.CrossOriginOpenerPolicy = CrossOriginOpenerPolicy.unsafe_none;
            opts.CrossOriginResourcePolicy = CrossOriginResourcePolicy.cross_origin;
            opts.ContentSecurityPolicyIgnoreUrls = new string[] { "index.html" };
        });

        // Act

        // Assert
        AssertConfigurationApplied(settings);
    }

    [Fact(DisplayName = "Empty action delegate should configure settings with defaults")]
    public void Empty_Action_Delegate_Should_Configure_Settings_With_Defaults()
    {
        // Arrange
        var settings = ApplicationBuilderExtensions.GetSettingsFromDelegate(null);

        // Act

        // Assert
        AssertDefaultApplied(settings);
    }

    [Fact(DisplayName = "IConfiguration should configure settings correctly")]
    public void IConfiguration_Should_Configure_Settings_Correctly()
    {
        // Arrange
        var inMemoryConfiguration = new Dictionary<string, string>
        {
            {"SecurityHeaders:XFrameOption", "sameorigin"},
            {"SecurityHeaders:XContentTypeOptions", "no_header"},
            {"SecurityHeaders:XPermittedCrossDomainPolicies", "all"},
            {"SecurityHeaders:ReferrerPolicy", "strict_origin"},
            {"SecurityHeaders:ClearSiteData:0", "*"},
            {"SecurityHeaders:CrossOriginEmbedderPolicy", "unsafe_none"},
            {"SecurityHeaders:CrossOriginOpenerPolicy", "unsafe_none"},
            {"SecurityHeaders:CrossOriginResourcePolicy", "cross_origin"},
            { "SecurityHeaders:ContentSecurityPolicyIgnoreUrls:0","index.html" }
        };

        ConfigurationBuilder builder = new();
        builder.AddInMemoryCollection(inMemoryConfiguration);
        var configuration = builder.Build();

        // Act
        var settings = ApplicationBuilderExtensions.GetSettingsFromConfiguration(configuration, "SecurityHeaders");

        // Assert
        AssertConfigurationApplied(settings);
    }

    [Fact(DisplayName = "Wrong section name should configure settings with defaults")]
    public void Wrong_Section_Name_Should_Configure_Settings_With_Defaults()
    {
        // Arrange
        var inMemoryConfiguration = new Dictionary<string, string>
        {
            {"WrongSectionName:XFrameOption", "sameorigin"},
            {"WrongSectionName:XContentTypeOptions", "none"},
            {"WrongSectionName:XPermittedCrossDomainPolicies", "all"},
            {"WrongSectionName:ReferrerPolicy", "strict_origin"},
            {"WrongSectionName:ClearSiteData:0", "*"},
            {"WrongSectionName:CrossOriginEmbedderPolicy", "unsafe_none"},
            {"WrongSectionName:CrossOriginOpenerPolicy", "unsafe_none"},
            {"WrongSectionName:CrossOriginResourcePolicy", "cross_origin"}
        };

        ConfigurationBuilder builder = new();
        builder.AddInMemoryCollection(inMemoryConfiguration);
        var configuration = builder.Build();

        // Act
        var settings = ApplicationBuilderExtensions.GetSettingsFromConfiguration(configuration, "SecurityHeaders");

        // Assert
        AssertDefaultApplied(settings);
    }

    private static void AssertConfigurationApplied(SecurityHeadersConfigurationSettings settings)
    {
        settings.Should().NotBeNull("settings must be initialized");
        settings.XFrameOption.Should().Be(XFrameOption.sameorigin, "XFrameOption passed value should be used");
        settings.XContentTypeOptions.Should().Be(XContentTypeOptions.no_header, "XContentTypeOptions passed value should be used");
        settings.XPermittedCrossDomainPolicies.Should().Be(XPermittedCrossDomainPolicies.all, "XPermittedCrossDomainPolicies passed value should be used");
        settings.ReferrerPolicy.Should().Be(ReferrerPolicy.strict_origin, "ReferrerPolicy passed value should be used");
        settings.ClearSiteData?.ToString().Should().Be("\"*\"", "ReferrerPolicy passed value should be used");
        settings.CrossOriginEmbedderPolicy.Should().Be(CrossOriginEmbedderPolicy.unsafe_none, "CrossOriginEmbedderPolicy passed value should be used");
        settings.CrossOriginOpenerPolicy.Should().Be(CrossOriginOpenerPolicy.unsafe_none, "CrossOriginOpenerPolicy passed value should be used");
        settings.CrossOriginResourcePolicy.Should().Be(CrossOriginResourcePolicy.cross_origin, "CrossOriginResourcePolicy passed value should be used");
        settings.ContentSecurityPolicyIgnoreUrls.Should().NotBeNull();
        settings.ContentSecurityPolicyIgnoreUrls.Should().HaveCount(1);
        settings.ContentSecurityPolicyIgnoreUrls.Should().Contain("index.html");
    }

    private static void AssertDefaultApplied(SecurityHeadersConfigurationSettings settings)
    {
        settings.Should().NotBeNull("settings must be initialized");
        settings.XFrameOption.Should().Be(XFrameOption.deny, "XFrameOption default value should be used");
        settings.XContentTypeOptions.Should().Be(XContentTypeOptions.nosniff, "XContentTypeOptions default value should be used");
        settings.XPermittedCrossDomainPolicies.Should().Be(XPermittedCrossDomainPolicies.none, "XPermittedCrossDomainPolicies default value should be used");
        settings.ReferrerPolicy.Should().Be(ReferrerPolicy.no_referrer, "ReferrerPolicy default value should be used");
        settings.ClearSiteData?.ToString().Should().Be("\"cache\",\"cookies\",\"storage\"", "ClearSiteData default value should be used");
        settings.CrossOriginEmbedderPolicy.Should().Be(CrossOriginEmbedderPolicy.require_corp, "CrossOriginEmbedderPolicy default value should be used");
        settings.CrossOriginOpenerPolicy.Should().Be(CrossOriginOpenerPolicy.same_origin, "CrossOriginOpenerPolicy default value should be used");
        settings.CrossOriginResourcePolicy.Should().Be(CrossOriginResourcePolicy.same_origin, "CrossOriginResourcePolicy default value should be used");
        settings.ContentSecurityPolicyIgnoreUrls.Should().NotBeNull();
        settings.ContentSecurityPolicyIgnoreUrls.Should().HaveCount(0);
    }

    [Fact(DisplayName = "AddNonceService_Register_Service")]
    public void AddNonceService_Register_Service()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        // Act        
        services.AddNonceService();
        var registeredService = services.BuildServiceProvider().GetService<INonceService>();

        // Assert            
        registeredService.Should().NotBeNull();
    }
}