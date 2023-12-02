using Bogus;
using FluentAssertions;
using Models;
using Models.Text;
using Moq;
using Services.Internals;
using Services.Internals.Models;
using Services.Ports;

namespace Services.Tests
{
    public class DataServiceTests
    {
        public readonly Mock<IImportDataParser> _importDataParserMock;
        public readonly Mock<ISearchStringParser> _searchStringParserMock;
        public readonly PretendBookDataSource _pretendBookDataSource;

        public DataServiceTests()
        {
            _importDataParserMock = new Mock<IImportDataParser>();
            _searchStringParserMock = new Mock<ISearchStringParser>();
            _pretendBookDataSource = new PretendBookDataSource();
        }

        private DataService buildService()
        {
            return new DataService(
                _importDataParserMock.Object,
                _searchStringParserMock.Object,
                _pretendBookDataSource
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
                    testData.AddPropertyValueData("roomId", Random.Shared.Next().ToString());
                    testData.AddPropertyValueData("rowId", Random.Shared.Next().ToString());
                    testData.AddPropertyValueData("shelfId", Random.Shared.Next().ToString());

                }

                yield return testData;
            }
        }

        private IEnumerable<Book> createRandomBooks(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var fake = new Faker();
                var book = new Book()
                {
                    Title = fake.Company.CompanyName(),
                    Publisher = fake.Hacker.Noun(),
                    Id = Guid.NewGuid(),
                    ISBN = fake.Hacker.Phrase(),
                    LibraryItemType = LibraryItemType.Book,
                    PageCount = Random.Shared.Next(100, 500),
                    YearPublished = Random.Shared.Next(100, 2100),
                    RoomId = Random.Shared.Next(),
                    RowId = Random.Shared.Next(),
                    ShelfId = Random.Shared.Next(),
                };
                for (var j = 0; j < Random.Shared.Next(1,5); j++)
                {
                    book.Authors.Add(new Faker().Person.FullName);
                }
                yield return book;
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

        #region FindBooks

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        public void FindBooks_ValidInputThatMatchAllBooks_ReturnsAllBooks(int booksToTest)
        {
            var searchText = "12345";

            var books = createRandomBooks(booksToTest).ToList();

            for (var i = 0; i < books.Count(); i++)
            {
                switch (i%8)
                {
                    case 0:
                        books[i].Title += searchText;
                        break;
                    case 1:
                        books[i].Authors[0] += searchText;
                        break;
                    case 2:
                        books[i].Publisher += searchText;
                        break;
                    case 3:
                        books[i].YearPublished = int.Parse(searchText);
                        break;
                    case 4:
                        books[i].ISBN += searchText;
                        break;
                    case 5:
                        books[i].RoomId = int.Parse(searchText);
                        break;
                    case 6:
                        books[i].RowId = int.Parse(searchText);
                        break;
                    case 7:
                        books[i].ShelfId = int.Parse(searchText);
                        break;
                }
            }

            _searchStringParserMock.Setup(s => s.ParseSearchString(searchText))
                .Returns(new List<ParsedSearchCondition>()
                {
                    new ParsedSearchCondition(searchText)
                });
            _pretendBookDataSource.Books = books;

            var result = buildService().FindBooks(searchText);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(booksToTest);
        }

        #endregion
    }
}