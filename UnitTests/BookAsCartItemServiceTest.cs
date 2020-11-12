using System.Threading.Tasks;
using Xunit;
using EntityFrameworkCore3Mock;
using MyAPI.Core.Contexts;
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Models.DbEntities;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Services;
using System.Linq;

namespace MyAPI.UnitTests
{
    //Unit tests for BookAsCartItemService
    public class BookAsCartItemServiceTest
    {
        //dummy options
        public DbContextOptions<MyAppDbContext> dummyOptions { get; } = new DbContextOptionsBuilder<MyAppDbContext>().Options;
        [Fact]
        public async Task CreateCartItem_AddsANewCartItem_ToBooksAsCartItemsTable()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            moq.CreateDbSetMock(x => x.BooksAsCartItems, new[]
            {
                new BookAsCartItem {Id = 1, Price = 10, Quantity = 2},

                new BookAsCartItem {Id = 2, Price = 20, Quantity = 1}

            });

            BookAsCartItemDTO new_book = new BookAsCartItemDTO()
            {
                Id = 3,

                Price = 15,

                Quantity = 4

            };

           //Act
           BookAsCartItemService service = new BookAsCartItemService(moq.Object);

           await service.CreateCartItem(new_book);

           //Assert
           Assert.Equal(3, moq.Object.BooksAsCartItems.Count());

        }

        [Fact]
        public async Task GetAllCartItems_WhenCalled_ReturnsAllCartItems()
        {
            //Arrange
            var my_Moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            my_Moq.CreateDbSetMock(x => x.BooksAsCartItems, new[]
            {
                new BookAsCartItem {Id = 1, Price = 10, Quantity = 2},

                new BookAsCartItem {Id = 2, Price = 20, Quantity = 1}

            });

            //Act
            BookAsCartItemService my_Service = new BookAsCartItemService(my_Moq.Object);

            await my_Service.GetAllCartItems();

            //Assert
            Assert.NotNull(my_Moq.Object.BooksAsCartItems);

        }

        [Fact]
        public void GetOneCartItem_WhenCalled_ReturnsOneCartItem()
        {
            //Arrange
            var context_Moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            context_Moq.CreateDbSetMock(x => x.BooksAsCartItems, new[]
            {
                new BookAsCartItem {Id = 1, Price = 10, Quantity = 2},

                new BookAsCartItem {Id = 2, Price = 20, Quantity = 1}

            });

            //Act
            BookAsCartItemService service = new BookAsCartItemService(context_Moq.Object);

            var chosen_Item1 = context_Moq.Object.BooksAsCartItems.FirstOrDefault(i => i.Id == 1);

            var chosen_Item2 = context_Moq.Object.BooksAsCartItems.FirstOrDefault(i => i.Id == 2);

            service.GetOneCartItem(1);

            service.GetOneCartItem(2);

            //Assert
            Assert.Equal(10, chosen_Item1.Price);

            Assert.Equal(2, chosen_Item1.Quantity);

            Assert.Equal(20, chosen_Item2.Price);

            Assert.Equal(1, chosen_Item2.Quantity);

        }

        [Fact]
        public async Task EditCatItem_Returns_UpdatedCartItem()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            moq.CreateDbSetMock(x => x.BooksAsCartItems, new[]
            {
                new BookAsCartItem {Id = 1, Price = 10, Quantity = 2},

                new BookAsCartItem {Id = 2, Price = 20, Quantity = 1}

            });

            BookAsCartItemDTO cartItemToBeUpdated = new BookAsCartItemDTO()
            {
                Id = 1,
                Quantity = 5 //we updated the Quantity of CartItem with Id 1 (we changed the Quantity from 2 to 5)
            };

            //Act
            BookAsCartItemService service = new BookAsCartItemService(moq.Object);

            BookAsCartItem cartItemWithId_1 = moq.Object.BooksAsCartItems.FirstOrDefault(i => i.Id == 1);

            await service.EditCartItem(cartItemToBeUpdated);

            //Assert
            Assert.Equal(5, cartItemWithId_1.Quantity);

        }

        [Fact]
        public async Task RemoveCartItem_DeletesCartItem_FromDatabase()
        {
            //Arrange
            var moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            moq.CreateDbSetMock(x => x.BooksAsCartItems, new[]
            {
                new BookAsCartItem {Id = 1, Price = 10, Quantity = 2},

                new BookAsCartItem {Id = 2, Price = 20, Quantity = 1}

            });

            //Act
            BookAsCartItemService service = new BookAsCartItemService(moq.Object);

            BookAsCartItem cartItemToBeDeleted = moq.Object.BooksAsCartItems.FirstOrDefault(i => i.Id == 2);

            await service.RemoveCartItem(2);

            //Assert
            Assert.Equal(1, moq.Object.BooksAsCartItems.Count());

        }
    }
}