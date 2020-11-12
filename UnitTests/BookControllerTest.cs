using Xunit;
using Moq;
using MyAPI.Core.Services;
using System.Collections.Generic;
using MyAPI.Core.Models.DTOs;
using System.Threading.Tasks;
using MyAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MyAPI.Core.Models.DbEntities;

namespace MyAPI.UnitTests
{
    //Unit tests for BookController
    public class BookControllerTest
    {
        //create dummy data
        private List<BookDTO> DummyBook()
        {
            List<BookDTO> my_List = new List<BookDTO>();//create a new list

            //create a new category
            Category category = new Category()
            {
                CategoryId = 1,
                CategoryName ="Novel"
            };

            //create a new book
            BookDTO myNewBook1 = new BookDTO()
            {
                Id = 1,

                Title = "Jerry's Success Story",

                Author = "James x",

                CategoryId = 1,

                Category = category,

                ISBN = 1234567890,

                Price = 15,

                DateOfPublication = "2018-06-03"
                
            };

            //create a new book
            BookDTO myNewBook2 = new BookDTO()
            {
                Id = 2,

                Title = "Story of Two Good Friends",

                Author = "Mary Y",

                CategoryId = 1,

                Category = category,

                ISBN = 1020304050,

                Price = 25,

                DateOfPublication = "2019-03-15"
               
            };

            //Add the two books to the list
            my_List.Add(myNewBook1);//add your first book to the list

            my_List.Add(myNewBook2);//add your second book to the list

            return my_List;   
        }

        //Test Post Method
        [Fact]
        public async Task PostABook_WhenValidObjectIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock <IBookService>();

            Category category = new Category()
            {
                CategoryId = 2,
                CategoryName = "Sports"
            };

            BookDTO book = new BookDTO()
            {
                Id = 3,

                Title = "Swimming Advantages",

                Author = "Alex T",

                CategoryId = 2,

                Category = category,

                ISBN = 9876543210,

                Price = 36,

                DateOfPublication ="2019-03-15"
               
            };
            
            //Return will give you some random number for Id
            moq.Setup(i => i.CreateBook(It.IsAny<BookDTO>())).Returns(() => Task.FromResult(book));

            //Act
            var controller = new BookController(moq.Object);

            var response = await controller.PostABook(book);

            //Assert
            Assert.IsType<OkObjectResult>(response);

            var created_response = Assert.IsType<OkObjectResult>(response);

            Assert.True( (created_response.Value as BookDTO).Id > 0 );
  
        }

        [Fact]
        public async Task BookModelValidation_AuthorNameRequired()
        {
            //Arrange
            var moq= new Mock <IBookService>();

            Category category = new Category()
            {
                CategoryId = 2,
                CategoryName = "Sports"
            };

            //This Book does not contain Author hence it is invalid
            BookDTO authorIsMissing = new BookDTO()
            {
                Id = 3,

                Title = "Swimming Advantages",

                CategoryId = 2,

                Category = category,

                ISBN = 9876543210,

                Price = 36,

                DateOfPublication ="2019-03-15"

            };

            moq.Setup(r => r.CreateBook(authorIsMissing));

            var controller = new BookController(moq.Object);//pass moq object inside controller

            controller.ModelState.AddModelError("Author","Required");

            //Act
            var result = await controller.PostABook(authorIsMissing);

            //Assert
            Assert.IsType<BadRequestResult>(result);

        }
        
        //Test Post method when null is passed
        [Fact]
        public async Task PostABook_WhenNullIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq =new Mock<IBookService>();

            moq.Setup(i => i.CreateBook(null));

            //Act
            var controller = new BookController(moq.Object);

            var result = await controller.PostABook(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
        
         
        //Test Get method
        [Fact]
        public async Task GetListContainingAllBooks_WhenCalledReturnsAllBooks()
        {
            //Arrange
            var moq = new Mock<IBookService>();

            moq.Setup(i => i.GetAllBooks()).ReturnsAsync(DummyBook());

            //Act
            var controller = new BookController(moq.Object);

            var all = await controller.GetListContainingAllBooks();

            //Assert
            Assert.NotNull(all);

        }
        
        //Test Get by id method when invalid object is passed
        [Fact]
        public void GetBookById_WhenInValidIdIsPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var moq = new Mock<IBookService>();

            moq.Setup(x => x.GetOneBook(0)).Returns(DummyBook().FirstOrDefault(i => i.Id == 0));

            //Act
            var controller = new BookController(moq.Object);

            var myResult = controller.GetBookById(0);//Id 0 is an invalid id

            //Assert
            Assert.IsType<NotFoundResult>(myResult.Result);

        }

        //Test Get by id method when valid object is passed
        [Fact]
        public void GetBookById_WhenValidIdIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<IBookService>();

            moq.Setup(x => x.GetOneBook(2)).Returns(DummyBook().FirstOrDefault(i => i.Id == 2));

            //Act
            var controller = new BookController(moq.Object);

            var myResult = controller.GetBookById(2);//Id 2 is a valid id

            //Assert
            Assert.IsType<OkObjectResult>(myResult.Result);

        }
        
        //Test get by id method when valid id is passed it should return correct book
        [Fact]
        public void GetBookById_WhenValidIdPassed_ReturnsCorrectBook()
        {
            //Arrange
            var moq = new Mock<IBookService>();

            moq.Setup(i => i.GetOneBook(1)).Returns(DummyBook().FirstOrDefault(x => x.Id == 1));

            //Act
            var controller = new BookController(moq.Object);

            var book = controller.GetBookById(1).Result as OkObjectResult;

            //Assert
            Assert.Equal("Jerry's Success Story", (book.Value as BookDTO).Title);
            
        }
        

        //Test Put method when null is passed
        [Fact]
        public async Task UpdateBook_WhenNullIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq = new Mock<IBookService>();

            moq.Setup(t => t.EditBook(null));

            //Act
            var controller = new BookController(moq.Object);

            var result = await controller.UpdateBook(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }

        //Test Put method when existing book is passed
        [Fact]
        public async Task UpdateBook_WhenValidBookIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<IBookService>();

            Category category = new Category()
            {
                CategoryId = 1,
                CategoryName ="Novel"
            };

            BookDTO book = new BookDTO()
            {
                Id = 2,

                Title = "Story of Two Good Friends",

                Author = "Mary Y",

                CategoryId = 1,

                Category = category,

                ISBN = 1020304050,

                Price = 25,

                DateOfPublication = "2019-03-15"
            };

            moq.Setup(r => r.EditBook(book));

            //Act
            var controller = new BookController(moq.Object);

            var ok_Result = await controller.UpdateBook(book);

            //Assert
            Assert.IsType <OkObjectResult>(ok_Result);

        }

        //Test Delete method when null is passed
        [Fact]
        public async Task DeleteBook_WhenInValidIdIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq = new Mock<IBookService>();

            moq.Setup(r => r.RemoveBook(null));

            //Act
            var controller = new BookController(moq.Object);

            var result = await controller.DeleteBook(null);

            //Assert
            Assert.IsType <NotFoundResult>(result);

        }

        //Test Delete method when valid id is passed
        [Fact]
        public async Task DeleteBook_WhenValidIdIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<IBookService>();

            moq.Setup(r => r.RemoveBook(1));

            //Act
            var controller = new BookController(moq.Object);

            var result = await controller.DeleteBook(1);

            //Assert
            Assert.IsType <OkResult>(result);

        }
    }  
}