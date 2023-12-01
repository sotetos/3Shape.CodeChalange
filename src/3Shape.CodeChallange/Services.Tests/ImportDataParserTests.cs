using FluentAssertions;
using Models;
using Services.Internals;

namespace Services.Tests
{
    public class ImportDataParserTests
    {
        private ImportDataParser buildService()
        {
            return new ImportDataParser();
        }

        #region ParseInput

        [Fact]
        public void ParseInput_ValidFixedInput_ReturnsMatchingData()
        {
            var inputCopiedFromAssignment =
                "Book:\r\nAuthor: Brian Jensen\r\nTitle: Texts from Denmark\r\nPublisher: Gyldendal\r\nPublished: 2001\r\nNumberOfPages: 253\r\n\u00a0\r\nBook:\r\nAuthor: Peter Jensen\r\nAuthor: Hans Andersen\r\nTitle: Stories from abroad\r\nPublisher: Borgen\r\nPublished: 2012\r\nNumberOfPages: 156";
            var result = buildService().ParseInput(inputCopiedFromAssignment);

            result.First().InputType.Should().Be(LibraryItemType.Book);
            result.First().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Author", "Brian Jensen"));
            result.First().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Title", "Texts from Denmark"));
            result.First().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Publisher", "Gyldendal"));
            result.First().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Published", "2001"));
            result.First().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("NumberOfPages", "253"));

            result.Last().InputType.Should().Be(LibraryItemType.Book);
            result.Last().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Author", "Peter Jensen"));
            result.Last().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Author", "Hans Andersen"));
            result.Last().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Title", "Stories from abroad"));
            result.Last().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Publisher", "Borgen"));
            result.Last().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("Published", "2012"));
            result.Last().PropertyValueData.Should()
                .ContainEquivalentOf(new KeyValuePair<string, string>("NumberOfPages", "156"));
        }

        [Theory]
        [InlineData(LibraryItemType.Book)]
        [InlineData(LibraryItemType.CD)]
        [InlineData(LibraryItemType.DVD)]
        [InlineData(LibraryItemType.BlueRay)]
        [InlineData(LibraryItemType.EBook)]
        [InlineData(LibraryItemType.AudioFile)]
        [InlineData(LibraryItemType.VideoFile)]
        [InlineData(LibraryItemType.Other)]
        public void ParseInput_TypeOtherThanBook_ReturnsDataWithError(LibraryItemType type)
        {
            
            var inputText =
                $"{type}:\r\nAuthor: Brian Jensen\r\nTitle: Texts from Denmark\r\nPublisher: Gyldendal\r\nPublished: 2001\r\nNumberOfPages: 253";
            var result = buildService().ParseInput(inputText);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().HasErrors.Should().BeFalse();
            result.First().InputType.Should().Be(type);
        }

        [Fact]
        public void ParseInput_InvalidInputType_ReturnsDataWithError()
        {
            var inputText =
                "Fake:\r\nAuthor: Brian Jensen\r\nTitle: Texts from Denmark\r\nPublisher: Gyldendal\r\nPublished: 2001\r\nNumberOfPages: 253";
            var result = buildService().ParseInput(inputText);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().HasErrors.Should().BeTrue();
        }

        [Fact]
        public void ParseInput_PropertyHasExtraColon_ReturnsDataWithError()
        {
            var inputText =
                "Book:\r\nAuthor: Brian Jen:sen\r\nTitle: Texts from Denmark\r\nPublisher: Gyldendal\r\nPublished: 2001\r\nNumberOfPages: 253";
            var result = buildService().ParseInput(inputText);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().HasErrors.Should().BeTrue();
        }

        [Fact]
        public void ParseInput_NullInput_ThrowsArgumentNullException()
        {
            buildService().Invoking(s => s.ParseInput(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ParseInput_EmptyInput_ThrowsArgumentException()
        {
            buildService().Invoking(s => s.ParseInput("   ")).Should().Throw<ArgumentException>();
        }

        #endregion
    }
}
