using HttpContextMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using QuokkaDev.SecurityHeaders.Csp;
using QuokkaDev.SecurityHeaders.PermissionPolicy;
using System.Threading.Tasks;
using Xunit;

namespace QuokkaDev.SecurityHeaders.Tests
{
    public class SecurityHeadersMiddlewareUnitTest
    {
        public SecurityHeadersMiddlewareUnitTest()
        {
        }

        [Fact(DisplayName = "All configured header should be applied")]
        public async Task All_Configured_Header_Should_Be_Applied()
        {
            // Arrange
            var delegateMock = new Mock<RequestDelegate>();
            delegateMock.Setup(m => m.Invoke(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            var settings = new SecurityHeadersConfigurationSettings()
            {
                XFrameOption = XFrameOption.sameorigin,
                XContentTypeOptions = XContentTypeOptions.nosniff,
                UseContentSecurityPolicy = true,
                UsePermissionPolicy = true,
                UseClearSiteData = true,
                XPermittedCrossDomainPolicies = XPermittedCrossDomainPolicies.none,
                ReferrerPolicy = ReferrerPolicy.no_referrer,
                CrossOriginEmbedderPolicy = CrossOriginEmbedderPolicy.require_corp,
                CrossOriginOpenerPolicy = CrossOriginOpenerPolicy.same_origin,
                CrossOriginResourcePolicy = CrossOriginResourcePolicy.same_origin,
                ClearSiteData = new ClearSitedata.ClearSiteData()
                    .ClearCache()
                    .ClearCookies()
                    .ClearStorage(),
                ContentSecurityPolicy = ContentSecurityPolicyBuilder.New()
                    .AddDefaultSrc(d => d.Self())
                    .Build(),
                PermissionPolicy = PermissionPolicyBuilder.New()
                    .AddAccelerometer(d => d.Self())
                    .Build()
            };

            var context = new HttpContextMock();
            IHeaderDictionary headerDictionary = new HeaderDictionary();
            context.ResponseMock.Mock.Setup(r => r.Headers).Returns(headerDictionary);
            SecurityHeadersMiddleware middleware = new(delegateMock.Object, settings, null);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.X_FRAME_OPTIONS, nameof(XFrameOption.sameorigin)), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.X_CONTENT_TYPE_OPTIONS, nameof(XContentTypeOptions.nosniff)), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CONTENT_SECURITY_POLICY, It.IsAny<StringValues>()), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.X_PERMITTED_CROSS_DOMAIN_POLICIES, nameof(XPermittedCrossDomainPolicies.none)), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.REFERRER_POLICY, ReferrerPolicy.no_referrer.DashReplace()), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CLEAR_SITE_DATA, "\"cache\",\"cookies\",\"storage\""), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CROSS_ORIGIN_EMBEDDER_POLICY, CrossOriginEmbedderPolicy.require_corp.DashReplace()), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CROSS_ORIGIN_OPENER_POLICY, CrossOriginOpenerPolicy.same_origin.DashReplace()), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CROSS_ORIGIN_RESOURCE_POLICY, CrossOriginResourcePolicy.same_origin.DashReplace()), Times.Once);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.PERMISSION_POLICY, It.IsAny<StringValues>()), Times.Once);
        }

        [Fact(DisplayName = "No headers should be honored")]
        public async Task No_Headers_Should_Be_Honored()
        {
            // Arrange
            var delegateMock = new Mock<RequestDelegate>();
            delegateMock.Setup(m => m.Invoke(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            var settings = new SecurityHeadersConfigurationSettings()
            {
                XFrameOption = XFrameOption.none,
                XContentTypeOptions = XContentTypeOptions.none,
                UseContentSecurityPolicy = false,
                UsePermissionPolicy = false,
                XPermittedCrossDomainPolicies = XPermittedCrossDomainPolicies.no_header,
                ReferrerPolicy = ReferrerPolicy.none,
                CrossOriginEmbedderPolicy = CrossOriginEmbedderPolicy.none,
                CrossOriginOpenerPolicy = CrossOriginOpenerPolicy.none,
                CrossOriginResourcePolicy = CrossOriginResourcePolicy.none,
                ClearSiteData = null
            };

            var context = new HttpContextMock();
            IHeaderDictionary headerDictionary = new HeaderDictionary();
            context.ResponseMock.Mock.Setup(r => r.Headers).Returns(headerDictionary);
            SecurityHeadersMiddleware middleware = new(delegateMock.Object, settings, null);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.X_FRAME_OPTIONS, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.X_CONTENT_TYPE_OPTIONS, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CONTENT_SECURITY_POLICY, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.X_PERMITTED_CROSS_DOMAIN_POLICIES, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.REFERRER_POLICY, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CLEAR_SITE_DATA, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CROSS_ORIGIN_EMBEDDER_POLICY, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CROSS_ORIGIN_OPENER_POLICY, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CROSS_ORIGIN_RESOURCE_POLICY, It.IsAny<StringValues>()), Times.Never);
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.PERMISSION_POLICY, It.IsAny<StringValues>()), Times.Never);
        }

        [Fact(DisplayName = "Nonce service should be called")]
        public async Task Nonce_Service_Should_Be_Called()
        {
            // Arrange
            var delegateMock = new Mock<RequestDelegate>();
            delegateMock.Setup(m => m.Invoke(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            var settings = new SecurityHeadersConfigurationSettings()
            {
                UseContentSecurityPolicy = true,
                ContentSecurityPolicy = ContentSecurityPolicyBuilder.New()
                    .AddDefaultSrc(d => d.Nonce())
                    .Build()
            };

            var context = new HttpContextMock();
            IHeaderDictionary headerDictionary = new HeaderDictionary();
            context.ResponseMock.Mock.Setup(r => r.Headers).Returns(headerDictionary);

            var ns = new Mock<INonceService>();
            ns.Setup(s => s.RequestNonce).Returns("mock-nonce");
            context.RequestServicesMock.Mock.Setup(rs => rs.GetService(typeof(INonceService))).Returns(ns.Object);
            SecurityHeadersMiddleware middleware = new(delegateMock.Object, settings, null);

            // Act
            await middleware.InvokeAsync(context);

            // Assert            
            context.ResponseMock.HeadersMock.Mock.Verify(d => d.Add(Constants.Headers.CONTENT_SECURITY_POLICY, It.IsAny<StringValues>()), Times.Once);
            ns.Verify(s => s.RequestNonce, Times.Once);
        }
    }
}
