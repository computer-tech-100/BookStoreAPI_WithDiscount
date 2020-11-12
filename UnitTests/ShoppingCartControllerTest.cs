using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Models.DbEntities;
using MyAPI.Core.Services;
using MyAPI.WebApi.Controllers;

namespace MyAPI.UnitTests
{
    public class ShoppingCartControllerTest
    {
        //create dummy data
        private List<BookAsCartItemDTO> DummyCartItems()
        {
            List<BookAsCartItemDTO> cartItems = new List<BookAsCartItemDTO>();//this will contain cart items

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

            cartItems.Add(cartItem1);

            cartItems.Add(cartItem2);

            return cartItems;
        }

        private ShoppingCartDTO DummyCart()
        {
            ShoppingCartDTO cart = new ShoppingCartDTO()
            {
                Id = 1,
                
                AllBooksInsideCart = DummyCartItems(),

                Total = 135,

                Discount = 135/10, //13.5

                GrandTotal = 1215/10 //121.5

            } ;

            return cart;
        }

        [Fact]
        public async Task GetShoppingCart_WhenCalled_ReturnsAllCartItems_AndGrandTotal()
        {
            //Arrange
            var moq = new Mock<IShoppingCartService>();

            moq.Setup(x => x.GetCart()).ReturnsAsync(DummyCart());

            //Act
            var controller = new ShoppingCartController(moq.Object);

            var my_Cart = await controller.GetShoppingCart();

            //Assert
            Assert.NotNull(my_Cart);

        }
    }
}