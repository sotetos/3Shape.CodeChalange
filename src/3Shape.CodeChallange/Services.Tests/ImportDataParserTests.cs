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

        #endregion
    }
}
