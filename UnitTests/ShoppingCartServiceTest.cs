using Xunit;
using EntityFrameworkCore3Mock;
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Contexts;
using System.Threading.Tasks;
using MyAPI.Core.Services;
using MyAPI.Core.Models.DbEntities;
using System.Collections.Generic;

namespace MyAPI.UnitTests
{
    //Unit tests for ShoppingCartService
    public class ShoppingCartServiceTest
    {
        //dummy options
        public DbContextOptions<MyAppDbContext> dummyOptions { get; } = new DbContextOptionsBuilder<MyAppDbContext>().Options;
        [Fact]
        public void GetCart_WhenCalled_ReturnsAllCartItems_AndGrandTotalWithoutDiscount_IfTotalAmountIsNotAboveThreshold()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            BookAsCartItem myCartItem1 = new BookAsCartItem()
            {
                Id = 1,

                Price = 23,

                Quantity = 2   
            };

            BookAsCartItem myCartItem2 = new BookAsCartItem()
            {
                Id = 2,

                Price = 32,

                Quantity = 1  
            };

            List<BookAsCartItem> All_Books_As_Cart_Items_No_Discount = new List<BookAsCartItem>();

            All_Books_As_Cart_Items_No_Discount.Add(myCartItem1);

            All_Books_As_Cart_Items_No_Discount.Add(myCartItem2);

            
            BookAsCartItem myCartItemOne = new BookAsCartItem()
            {
                Id = 1,

                Price = 50,

                Quantity = 2   
            };

            BookAsCartItem myCartItemTwo = new BookAsCartItem()
            {
                Id = 2,

                Price = 60,

                Quantity = 1  
            };

            List<BookAsCartItem> All_Books_As_Cart_Items_With_Discount = new List<BookAsCartItem>();

            All_Books_As_Cart_Items_With_Discount.Add(myCartItemOne);

            All_Books_As_Cart_Items_With_Discount.Add(myCartItemTwo);

            moq.CreateDbSetMock(x => x.Carts, new[]
            {
                //When Total amount is not more than 100 dollors then we do not have discount i.e Total = GrandTotal
                new ShoppingCart { Id = 1, AllBooksInsideCart = All_Books_As_Cart_Items_No_Discount, Total = 78 , Discount = 0 , GrandTotal = 78 }
               
            });

            //Act 
            ShoppingCartService service = new ShoppingCartService(moq.Object);

            var my_Cart = service.GetCart();

            //Assert

            Assert.NotNull(my_Cart);

        }

        [Fact]
        public void GetCart_WhenCalled_ReturnsAllCartItems_AndGrandTotalWithDiscount_IfTotalAmountIsAboveThreshold()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);
            
            BookAsCartItem myCartItemOne = new BookAsCartItem()
            {
                Id = 1,

                Price = 50,

                Quantity = 2   
            };

            BookAsCartItem myCartItemTwo = new BookAsCartItem()
            {
                Id = 2,

                Price = 60,

                Quantity = 1  
            };

            List<BookAsCartItem> All_Books_As_Cart_Items_With_Discount = new List<BookAsCartItem>();

            All_Books_As_Cart_Items_With_Discount.Add(myCartItemOne);

            All_Books_As_Cart_Items_With_Discount.Add(myCartItemTwo);

            moq.CreateDbSetMock(x => x.Carts, new[]
            {
                //When Total amount is more than 100 dollors then we  have 10% discount 
                new ShoppingCart { Id = 1, AllBooksInsideCart = All_Books_As_Cart_Items_With_Discount, Total = 160 , Discount = 16 , GrandTotal = 144 }
               
            });

            //Act 
            ShoppingCartService service = new ShoppingCartService(moq.Object);

            var my_Cart = service.GetCart();

            //Assert
            Assert.NotNull(my_Cart);

        }
    }
}