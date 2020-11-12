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
    public class BookAsCartItemControllerTest
    {
        //create dummy data
        private List<BookAsCartItemDTO> DummyCartItem()
        {
            List<BookAsCartItemDTO> my_List = new List<BookAsCartItemDTO>();//create a new list

            //Create a new category
            Category category = new Category()
            {
                CategoryId = 1,
                CategoryName = "Novel"
            };

            //create a new book
            Book book1 = new Book()
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
            Book book2 = new Book()
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

            BookAsCartItemDTO cartItem1 = new BookAsCartItemDTO()
            {
                Id = 1,

                Book = book1,

                Price = 15,

                Quantity = 4

            };

            BookAsCartItemDTO cartItem2 = new BookAsCartItemDTO()
            {
                Id = 2,

                Book = book2,

                Price = 25,

                Quantity = 3

            };

            my_List.Add(cartItem1);//add cart item to the list

            my_List.Add(cartItem2);//add cart item to the list

            return my_List;   

        }

        //Test Post Method
        [Fact]
        public async Task PostACartItem_WhenValidObjectIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock <IBookAsCartItemService>();

            Category category = new Category()
            {
                CategoryId = 1,
                CategoryName = "Novel"
            };

            //create a new book
            Book book = new Book()
            {
                Id = 3,

                Title = "Jerry's Travel Story",

                Author = "Maria x",

                CategoryId = 1,

                Category = category,

                ISBN = 1486910263,

                Price = 34,

                DateOfPublication = "2016-10-27"
                
            };

            BookAsCartItemDTO cartItem = new BookAsCartItemDTO()
            {
                Id = 2,

                Book = book,

                Price = 34,

                Quantity = 1
            };
            
            moq.Setup(i => i.CreateCartItem(cartItem));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var response = await controller.PostACartItem(cartItem);

            //Assert
            Assert.IsType<OkObjectResult>(response);
  
        }
        
        //Test Post method when null cart item is passed
        [Fact]
        public async Task PostACartItem_WhenNullIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq =new Mock<IBookAsCartItemService>();

            moq.Setup(i => i.CreateCartItem(null));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var result = await controller.PostACartItem(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
        
        //Test Get method
        [Fact]
        public async Task GetListContainingAllCartItems_WhenCalledReturnsAllCartItems()
        {
            //Arrange
            var moq = new Mock<IBookAsCartItemService>();

            moq.Setup(i => i.GetAllCartItems()).ReturnsAsync(DummyCartItem());

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var all = await controller.GetListContainingAllCartItems();

            //Assert
            Assert.NotNull(all);

        }
        
        
        //Test Get by id method when invalid object is passed
        [Fact]
        public void GetCartItemById_WhenInValidIdIsPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var moq = new Mock<IBookAsCartItemService>();

            moq.Setup(x => x.GetOneCartItem(0)).Returns(DummyCartItem().FirstOrDefault(i => i.Id == 0));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var myResult = controller.GetCartItemById(0);//Id 0 is an invalid id

            //Assert
            Assert.IsType<NotFoundResult>(myResult.Result);

        }

        //Test Get by id method when valid object is passed
        [Fact]
        public void GetCartItemById_WhenValidIdIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<IBookAsCartItemService>();

            moq.Setup(x => x.GetOneCartItem(2)).Returns(DummyCartItem().FirstOrDefault(i=>i.Id == 2));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var myResult = controller.GetCartItemById(2);//Id 2 is a valid id

            //Assert
            Assert.IsType<OkObjectResult>(myResult.Result);

        }
        
        //Test get by id method when valid id is passed it should return correct cart item
        [Fact]
        public void GetCartItemById_WhenValidIdPassed_ReturnsCorrectCartItem()
        {
            //Arrange
            var moq = new Mock<IBookAsCartItemService>();

            moq.Setup(i => i.GetOneCartItem(1)).Returns(DummyCartItem().FirstOrDefault(x => x.Id == 1));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var cart_Item = controller.GetCartItemById(1).Result as OkObjectResult;

            //Assert
            Assert.Equal(4, (cart_Item.Value as BookAsCartItemDTO).Quantity);
            
        }

        //Test Put method when null is passed
        [Fact]
        public async Task UpdateCartItem_WhenNullIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq = new Mock<IBookAsCartItemService>();

            moq.Setup(t => t.EditCartItem(null));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var result = await controller.UpdateCartItem(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }

        //Test Put method when existing cart item is passed
        [Fact]
        public async Task UpdateCartItem_WhenValidCartItemIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<IBookAsCartItemService>();

            Category category = new Category()
            {
                CategoryId = 1,
                CategoryName = "Novel"
            };

            //create a new book
            Book book = new Book()
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

            BookAsCartItemDTO cartItem = new BookAsCartItemDTO()
            {
                Id = 1,

                Book = book,

                Price = 15,

                Quantity = 4

            };

            moq.Setup(r => r.EditCartItem(cartItem));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var ok_Result = await controller.UpdateCartItem(cartItem);

            //Assert
            Assert.IsType <OkObjectResult>(ok_Result);

        }

        //Test Delete method when null is passed
        [Fact]
        public async Task DeleteCartItem_WhenInValidIdIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq = new Mock<IBookAsCartItemService>();

            moq.Setup(r => r.RemoveCartItem(null));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var result = await controller.DeleteCartItem(null);

            //Assert
            Assert.IsType <NotFoundResult>(result);

        }

        //Test Delete method when valid id is passed
        [Fact]
        public async Task DeleteCartItem_WhenValidIdIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<IBookAsCartItemService>();

            moq.Setup(r => r.RemoveCartItem(1));

            //Act
            var controller = new BookAsCartItemController(moq.Object);

            var result = await controller.DeleteCartItem(1);

            //Assert
            Assert.IsType <OkResult>(result);

        }
    }
}