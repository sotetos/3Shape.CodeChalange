using Bogus;
using FluentAssertions;
using Models;
using Models.Text;
using Moq;
using Services.DTOs;
using Services.Internals;
using Services.Ports;

namespace Services.Tests
{
    public class DataServiceTests
    {
        private readonly Mock<IImportDataParser> _importDataParserMock;
        private readonly Mock<ISearchStringParser> _searchStringParserMock;
        private readonly PretendBookDataSource _pretendBookDataSource;

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
        public void ReadBooks_FixedInputWithWeirdCapitals_ReturnsMatchingBook()
        {
            var inputData = new ParsedInputData()
            {
                InputType = LibraryItemType.Book
            };
            var fake = new Faker();
            var Title = fake.Company.CatchPhrase();
            inputData.AddPropertyValueData("TITLE", Title);
            var publisher = fake.Company.CompanyName();
            inputData.AddPropertyValueData("pubLishEr", publisher);
            var ISBN = fake.Hacker.Adjective();
            inputData.AddPropertyValueData("iSbN", ISBN);
            var pageCount = Random.Shared.Next();
            inputData.AddPropertyValueData("numberofpages", pageCount.ToString());
            var yearPublished = Random.Shared.Next();
            inputData.AddPropertyValueData("published", yearPublished.ToString());
            var room = Random.Shared.Next();
            inputData.AddPropertyValueData("roomId", room.ToString());
            var row = Random.Shared.Next();
            inputData.AddPropertyValueData("ROWid", row.ToString());
            var shelf = Random.Shared.Next();
            inputData.AddPropertyValueData("ShElFiD", shelf.ToString());
            var author1 = fake.Person.FullName;
            inputData.AddPropertyValueData("auTHOR", author1);
            var author2 = new Faker().Person.FullName;
            inputData.AddPropertyValueData("auTHOR", author2);

            _importDataParserMock.Setup(i => i.ParseInput(It.IsAny<string>()))
                .Returns(new List<ParsedInputData>(){inputData});

            var result = buildService().ReadBooks("this text doesn't matter in this test.");

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);

            result.First().Title.Should().Be(Title);
            result.First().Publisher.Should().Be(publisher);
            result.First().ISBN.Should().Be(ISBN);
            result.First().PageCount.Should().Be(pageCount);
            result.First().YearPublished.Should().Be(yearPublished);
            result.First().RoomId.Should().Be(room);
            result.First().RowId.Should().Be(row);
            result.First().ShelfId.Should().Be(shelf);
            result.First().Authors.First().Should().Be(author1);
            result.First().Authors.Last().Should().Be(author2);
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

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        public void FindBooks_ValidInputThatMatch1Book_Returns1Book(int booksToTest)
        {
            var searchText = new Faker().Rant.Review();

            var books = createRandomBooks(booksToTest).ToList();

            books.Last().Title += searchText;

            _searchStringParserMock.Setup(s => s.ParseSearchString(searchText))
                .Returns(new List<ParsedSearchCondition>()
                {
                    new ParsedSearchCondition(searchText)
                });
            _pretendBookDataSource.Books = books;

            var result = buildService().FindBooks(searchText);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().Title.Should().EndWith(searchText);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        public void FindBooks_ValidInputThatMatchesSomeBooks_Returns1Book(int booksToTest)
        {

            var books = createRandomBooks(booksToTest).ToList();

            var searchText = books[4].Authors.First();

            _searchStringParserMock.Setup(s => s.ParseSearchString(searchText))
                .Returns(new List<ParsedSearchCondition>()
                {
                    new ParsedSearchCondition(searchText)
                });
            _pretendBookDataSource.Books = books;

            var result = buildService().FindBooks(searchText);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCountGreaterOrEqualTo(1);
            foreach (var book in result)
            {
                book.Authors.Any(a => a.Equals(searchText)).Should().BeTrue();
            }
        }

        [Fact]
        public void FindBooks_EmptyInput_ReturnsAllBooks()
        {
            var searchText = string.Empty;

            var books = createRandomBooks(50).ToList();

            _searchStringParserMock.Setup(s => s.ParseSearchString(searchText))
                .Returns(new List<ParsedSearchCondition>(1));
            _pretendBookDataSource.Books = books;

            var result = buildService().FindBooks(searchText);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(50);
        }

        [Fact]
        public void FindBooks_NullInput_ThrowsArgumentNullException()
        {
            buildService().Invoking(s => s.FindBooks(null)).Should().Throw<ArgumentNullException>();
        }

        #endregion
    }
}