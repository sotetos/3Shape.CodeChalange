using FluentAssertions;
using Services.Internals;
using System.Diagnostics.Metrics;

namespace Services.Tests
{
    public class SearchStringParserTests
    {
        private SearchStringParser buildService()
        {
            return new SearchStringParser();
        }

        [Fact]
        public void ParseSearchString_ValidSingleInput_ReturnsResult()
        {
            var input = "*20*";

            var result = buildService().ParseSearchString(input);
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().StringValue.Should().Be("20");
            result.First().IntegerValue.Should().Be(20);
        }

        [Fact]
        public void ParseSearchString_ValidDoubleInput_ReturnsResult()
        {
            var input = "*20* & *peter*";

            var result = buildService().ParseSearchString(input);
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.First().StringValue.Should().Be("20");
            result.First().IntegerValue.Should().Be(20);
            result.Last().StringValue.Should().Be("peter");
            result.Last().IntegerValue.Should().BeNull();
        }

        [Fact]
        public void ParseSearchString_AmpersandInAnEntry_ReturnsResultWithAmpersand()
        {
            var input = "*20* & *pet\\&er*";

            var result = buildService().ParseSearchString(input);
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.First().StringValue.Should().Be("20");
            result.First().IntegerValue.Should().Be(20);
            result.Last().StringValue.Should().Be("pet\\&er");
            result.Last().IntegerValue.Should().BeNull();
        }

        [Fact]
        public void ParseSearchString_NullInput_ReturnsNullArgumentException()
        {
            buildService().Invoking(v => v.ParseSearchString(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ParseSearchString_EmptyInput_ReturnsEmptyResults()
        {
            var result = buildService().ParseSearchString(string.Empty);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
