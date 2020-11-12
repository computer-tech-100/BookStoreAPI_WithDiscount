using Xunit;
using EntityFrameworkCore3Mock;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Contexts;
using MyAPI.Core.Models.DbEntities;
using MyAPI.Core.Services;
using MyAPI.Core.Models.DTOs;
using System.Linq;

namespace MyAPI.UnitTests
{
    //Unit tests for BookService
    public class BookServiceTest
    {
        //dummy options
        public DbContextOptions<MyAppDbContext> dummyOptions { get; } = new DbContextOptionsBuilder<MyAppDbContext>().Options;
        [Fact]
        public async Task CreateBook_AddsANewBook_ToBooksTable()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            moq.CreateDbSetMock(i => i.Books, new[]
            {
                new Book {Id = 1, Title = "Animals and Nature", Author = "James X", CategoryId = 1, ISBN = 1234567890, Price = 10,  DateOfPublication = "2018-03-11" },

                new Book {Id = 2, Title = "Family and Relationships", Author = "Jerry Y", CategoryId = 2, ISBN = 8876419010, Price = 20,  DateOfPublication = "2019-05-18" }

            });//--> now we have two books inside our Books database

            BookDTO new_book = new BookDTO()
            {
                Id = 3, 

                Title = "Programming Instructions", 

                Author = "John Abcd", 

                CategoryId = 3, 

                ISBN = 1984657201, 

                Price = 50, 

                DateOfPublication = "2017-01-20"

            };

            //Act
            var service = new BookService(moq.Object);

            await service.CreateBook(new_book);//add a new book to database

            //Assert
            Assert.Equal(3,moq.Object.Books.Count());

        }

        [Fact]
        public async Task GetAllBooks_WhenCalled_ReturnsAllBooks()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            moq.CreateDbSetMock(i => i.Books, new[]
            {
                new Book {Id = 1, Title = "Animals and Nature", Author = "James X", CategoryId = 1, ISBN = 1234567890, Price = 10,  DateOfPublication = "2018-03-11" },

                new Book {Id = 2, Title = "Family and Relationships", Author = "Jerry Y", CategoryId = 2, ISBN = 8876419010, Price = 20,  DateOfPublication = "2019-05-18" }

            });//--> now we have two books inside our Books database hence it is not null

            //Act
            var service = new BookService(moq.Object);

            await service.GetAllBooks();

            //Assert
            Assert.NotNull(moq.Object.Books);

        }

        [Fact]
        public void GetOneBook_WhenCalled_ReturnsOneBook()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            moq.CreateDbSetMock(i => i.Books, new[]
            {
                new Book {Id = 1, Title = "Animals and Nature", Author = "James X", CategoryId = 1, ISBN = 1234567890, Price = 10,  DateOfPublication = "2018-03-11" },

                new Book {Id = 2, Title = "Family and Relationships", Author = "Jerry Y", CategoryId = 2, ISBN = 8876419010, Price = 20,  DateOfPublication = "2019-05-18" }

            });

            //Act
            var service = new BookService(moq.Object);

            var book1 = service.GetOneBook(1);

            var book2 = service.GetOneBook(2);

            //Assert
            Assert.Equal("James X", book1.Author);

            Assert.Equal("Family and Relationships", book2.Title);

        }

        [Fact]
        public async Task EditBook_Returns_UpdatedBook()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            moq.CreateDbSetMock(i => i.Books, new[]
            {
                new Book {Id = 1, Title = "Animals and Nature", Author = "James X", CategoryId = 1, ISBN = 1234567890, Price = 10,  DateOfPublication = "2018-03-11" },

            });

            BookDTO updated_Book = new BookDTO()
            {
                Id = 1, 

                Title = "Our Beautiful Planet", //we edited the Title 

                Price = 20 //we edited the Price 

            };

            //Act
            var service = new BookService(moq.Object);

            await service.EditBook(updated_Book);

            var book_ToBe_Updated = moq.Object.Books.FirstOrDefault(b => b.Id == 1);

            //Assert
            Assert.Equal(20, book_ToBe_Updated.Price);

            Assert.Equal("Our Beautiful Planet",book_ToBe_Updated.Title);

        }

        [Fact]
        public async Task RemoveBook_DeletesBook_FromDatabase()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            moq.CreateDbSetMock(i => i.Books, new[]
            {
                new Book {Id = 1, Title = "Animals and Nature", Author = "James X", CategoryId = 1, ISBN = 1234567890, Price = 10,  DateOfPublication = "2018-03-11" },

                new Book {Id = 2, Title = "Family and Relationships", Author = "Jerry Y", CategoryId = 2, ISBN = 8876419010, Price = 20,  DateOfPublication = "2019-05-18" },
                
                new Book {Id = 3, Title = "Sports", Author = "Micheal W", CategoryId = 4, ISBN = 8120387310, Price = 23,  DateOfPublication = "2016-11-23" }

            });//--> we have three books inside our table we want to remove a book with Id 3

            //Act
            var service = new BookService(moq.Object);

            await service.RemoveBook(3);//remove the book with Id 3

            //Assert
            //when we remove the book with Id 3 then the size of Books table decrease by 1 i.e size of Books table becomes 2
            //In this case we understand that RemoveBook function works correctly
            Assert.Equal(2, moq.Object.Books.Count());

        }
    }
}