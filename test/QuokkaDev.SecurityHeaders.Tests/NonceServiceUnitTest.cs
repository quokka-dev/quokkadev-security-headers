using FluentAssertions;
using QuokkaDev.SecurityHeaders.Csp;
using Xunit;

namespace QuokkaDev.SecurityHeaders.Tests
{
    public class NonceServiceUnitTest
    {
        public NonceServiceUnitTest()
        {
        }

        [Fact]
        public void Nonce_Should_Be_Unique()
        {
            // Arrange
            NonceService n1 = new NonceService();
            NonceService n2 = new NonceService();
            // Act


            // Assert
            n1.RequestNonce.Should().NotBe(n2.RequestNonce);
        }
    }
}
