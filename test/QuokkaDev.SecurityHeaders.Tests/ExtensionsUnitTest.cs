using FluentAssertions;
using HttpContextMoq;
using Microsoft.AspNetCore.Http;
using Moq;
using QuokkaDev.SecurityHeaders.Csp;
using System;
using Xunit;

namespace QuokkaDev.SecurityHeaders.Tests
{
    public class ExtensionsUnitTest
    {
        public ExtensionsUnitTest()
        {
        }

        [Fact(DisplayName = "DashReplace should works as expected")]
        public void DashReplace_Should_Works_As_Expected()
        {
            // Arrange
            var mock = new Mock<object>();
            mock.Setup(m => m.ToString()).Returns("string_with_underscore");
            var obj = mock.Object;

            // Act
            var result = obj.DashReplace();

            // Assert
            result.Should().Be("string-with-underscore");
        }

        [Fact(DisplayName = "DashReplace return null for null objects")]
        public void DashReplace_Return_Null_For_Null_Objects()
        {
            // Arrange            

            // Act
            var result = Extensions.DashReplace(null!);

            // Assert
            result.Should().BeNull();
        }

        [Fact(DisplayName = "DashReplace works for null for null strings")]
        public void DashReplace_Works_For_Null_For_Null_Strings()
        {
            // Arrange            
            var mock = new Mock<object>();
            mock.Setup(m => m.ToString()).Returns((string)null!);
            var obj = mock.Object;
            // Act
            var result = obj.DashReplace();

            // Assert
            result.Should().BeNull();
        }

        [Fact(DisplayName = "GetNonce works as expected")]
        public void GetNonce_Works_As_Expected()
        {
            // Arrange
            var context = new HttpContextMock();

            var ns = new Mock<INonceService>();
            ns.Setup(s => s.RequestNonce).Returns("mock-nonce");
            context.RequestServicesMock.Mock.Setup(rs => rs.GetService(typeof(INonceService))).Returns(ns.Object);

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(m => m.HttpContext).Returns(context);

            // Act
            string nonce = accessorMock.Object.GetNonce();

            // Assert            
            nonce.Should().Be("mock-nonce");
        }

        [Fact(DisplayName = "Unconfigured nonce service throw exception")]
        public void Unconfigured_Nonce_Service_Throw_Exception()
        {
            // Arrange
            var context = new HttpContextMock();

            context.RequestServicesMock.Mock.Setup(rs => rs.GetService(typeof(INonceService))).Returns((INonceService)null!);

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(m => m.HttpContext).Returns(context);

            // Act
            var result = () => accessorMock.Object.GetNonce();

            // Assert            
            result.Should().Throw<InvalidOperationException>().WithMessage("NonceService is not configured");
        }

        [Fact(DisplayName = "GetNonceAttribute works as expected")]
        public void GetNonceAttribute_Works_As_Expected()
        {
            // Arrange
            var context = new HttpContextMock();

            var ns = new Mock<INonceService>();
            ns.Setup(s => s.RequestNonce).Returns("mock-nonce");
            context.RequestServicesMock.Mock.Setup(rs => rs.GetService(typeof(INonceService))).Returns(ns.Object);

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(m => m.HttpContext).Returns(context);

            // Act
            string nonce = accessorMock.Object.GetNonceAttribute();

            // Assert            
            nonce.Should().Be("nonce=\"mock-nonce\"");
        }
    }
}
