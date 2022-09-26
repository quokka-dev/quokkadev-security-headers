using FluentAssertions;
using QuokkaDev.SecurityHeaders.PermissionPolicy;
using Xunit;

namespace QuokkaDev.SecurityHeaders.Tests
{
    public class PermissionPolicyDirectiveUnitTest
    {
        public PermissionPolicyDirectiveUnitTest()
        {
        }

        [Fact(DisplayName = "Empty allowed sources should works as expected")]
        public void Empty_Allowed_Sources_Should_Works_As_Expected()
        {
            // Arrange
            Directive directive = new() { Name = "battery" };

            // Act
            string result = directive.ToString();

            // Assert
            result.Should().Be("battery=()");
        }
    }
}
