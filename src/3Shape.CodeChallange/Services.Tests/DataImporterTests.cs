using Bogus;
using FluentAssertions;
using Models;
using Moq;
using Services.Internals;
using Services.Internals.Models;
using Services.Ports;

namespace Services.Tests
{
    public class DataImporterTests
    {
        public readonly Mock<IImportDataParser> _importDataParserMock;
        public readonly Mock<ISearchStringParser> _searchStringParserMock;
        public readonly Mock<PretendBookDataSource> _pretendBookDataSourceMock;

        public DataImporterTests()
        {
            _importDataParserMock = new Mock<IImportDataParser>();
            _searchStringParserMock = new Mock<ISearchStringParser>();
            _pretendBookDataSourceMock = new Mock<PretendBookDataSource>();
        }

        private DataImporter buildService()
        {
            return new DataImporter(
                _importDataParserMock.Object,
                _searchStringParserMock.Object,
                _pretendBookDataSourceMock.Object
            );
        }

        private IEnumerable<ParsedInputData> createRandomParsedInputData(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var fake = new Faker();
                var testData = new ParsedInputData()
                {
                    InputType = (LibraryItemType)(i % Enum.GetValues<LibraryItemType>().Length),
                };

                //LibraryItemBase properties
                testData.AddPropertyValueData("Title", fake.Music.Genre());

                //Only book is implemented, but a switch statement could be used to implement more
                if (testData.InputType == LibraryItemType.Book)
                {
                    for (var j = 0; j < Random.Shared.Next(1, 5); j++)
                    {
                        var fakeAuthor = new Faker();
                        testData.AddPropertyValueData("Author", fakeAuthor.Person.FullName);
                    }

                    testData.AddPropertyValueData("Publisher", fake.Company.CompanyName());
                    testData.AddPropertyValueData("Published", Random.Shared.Next(1200, 2100).ToString());
                    testData.AddPropertyValueData("NumberOfPages", Random.Shared.Next(100, int.MaxValue).ToString());
                }

                yield return testData;
            }
        }

        #region ReadBooks

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(200)]
        public void ReadBooks_ValidInputs_ReturnsBooks(int amountOfDataToGenerate)
        {
            var inputString = "fake";
            var fakeData = createRandomParsedInputData(amountOfDataToGenerate);

            _importDataParserMock.Setup(i => i.ParseInput(inputString))
                .Returns(fakeData);

            var service = buildService();
            var result = service.ReadBooks(inputString);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(fakeData.Count(f => f.InputType == LibraryItemType.Book));
        }

        [Fact]
        public void ReadBooks_NullInput_ThrowsArgumentNullException()
        {
            buildService().Invoking(s => s.ReadBooks(null)).Should().Throw<ArgumentNullException>();
        }

        #endregion

    }
}