using FluentAssertions;
using QuokkaDev.SecurityHeaders.ClearSitedata;
using System.Collections.Generic;
using Xunit;

namespace QuokkaDev.SecurityHeaders.Tests
{
    public class ClearSiteDataUnitTest
    {
        public ClearSiteDataUnitTest()
        {
        }

        [Fact(DisplayName = "Unconfigured object should return empty string")]
        public void Unconfigured_Object_Should_Return_Empty_String()
        {
            // Arrange
            ClearSiteData clearSiteData = new();

            // Act            

            // Assert
            clearSiteData.ToString().Should().BeEmpty();
        }

        [Theory(DisplayName = "Configured object should return expected string")]
        [MemberData(nameof(GetValues))]
        public void Configured_Object_Should_Return_Expected_String(IEnumerable<string> values, string expectedString)
        {
            // Arrange
            ClearSiteData clearSiteData = new(values);

            // Act            

            // Assert
            clearSiteData.ToString().Should().Be(expectedString);
        }

        public static IEnumerable<object[]> GetValues()
        {
            yield return new object[] { new string[] { "*" }, "\"*\"" };
            yield return new object[] { new string[] { "cache" }, "\"cache\"" };
            yield return new object[] { new string[] { "cookies" }, "\"cookies\"" };
            yield return new object[] { new string[] { "storage" }, "\"storage\"" };
            yield return new object[] { new string[] { "executionContexts" }, "\"executionContexts\"" };
            yield return new object[] { new string[] { "cache", "cookies" }, "\"cache\",\"cookies\"" };
            yield return new object[] { new string[] { "cache", "storage" }, "\"cache\",\"storage\"" };
            yield return new object[] { new string[] { "cache", "cookies", "storage" }, "\"cache\",\"cookies\",\"storage\"" };
            yield return new object[] { new string[] { "invalid" }, "" };
            yield return new object[] { new string[] { "cache", "invalid" }, "\"cache\"" };
            yield return new object[] { new string[] { "invalid", "cache" }, "\"cache\"" };
            yield return new object[] { new string[] { "cache", "cache", "cache" }, "\"cache\"" };
        }
    }
}
