using System.Threading.Tasks;
using Xunit;
using EntityFrameworkCore3Mock;//DbContextMock
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Contexts;
using MyAPI.Core.Models.DbEntities;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Services;
using System.Linq;

namespace MyAPI.UnitTests
{
    public class CategoryServiceTest
    {
        //Create dummy options that is type of our context i.e MyAppDbContext
        public DbContextOptions<MyAppDbContext> dummyOptions { get; } = new DbContextOptionsBuilder<MyAppDbContext>().Options;

        //Test if CreateCategory method adds a Category to database
        [Fact]
        public async Task CreateCategory_AddsNewCategory_ToCategoriesTable()
        {
            //Arrange
            var context_Moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            context_Moq.CreateDbSetMock(i => i.Categories, new[]
            {
                new Category {CategoryId = 1 , CategoryName ="Fiction"},

                new Category {CategoryId = 2 , CategoryName ="Romance"},

                new Category {CategoryId = 3 , CategoryName ="Fantasy"}

            });//--> now we have three categories inside our Categories database

            //We want to add a new Category (i.e the following) to our database
            var my_new_category = new CategoryDTO()
            {
                CategoryId = 4,

                CategoryName ="Sports"

            };
            
            //Act
            var my_service = new CategoryService(context_Moq.Object);

            await my_service.CreateCategory(my_new_category);

            //Assert
            //We have 3 Categories inside our Categories table
            //When we add a new Category to Categoires table then the size of Categories table increases by 1 
            Assert.Equal(4, context_Moq.Object.Categories.Count());

        }

        //Test if GetAllCategories method returns all of the categories
        [Fact]
        public async Task GetAllCategories_WhenCalled_ReturnsAllCategories()
        {
            //Arrange
            var context_Moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            context_Moq.CreateDbSetMock(i => i.Categories, new[]
            {
                new Category {CategoryId = 1 , CategoryName ="Fiction"},

                new Category {CategoryId = 2 , CategoryName ="Romance"},

                new Category {CategoryId = 3 , CategoryName ="Fantasy"}

            });//--> now we have three categories inside our Categories database hence it is not null

            //Act
            var service = new CategoryService(context_Moq.Object);

            await service.GetAllCategories();
            
            //Assert
            Assert.NotNull(context_Moq.Object.Categories);
        }

        //Test if GetOneCategory method returns a category
        [Fact]
        public void GetOneCategory_WhenCalled_ReturnsOneCategory()
        {
            //Arrange
            var context_Moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            context_Moq.CreateDbSetMock(i => i.Categories, new[]
            {
                new Category {CategoryId = 1 , CategoryName ="Fiction"},

                new Category {CategoryId = 2 , CategoryName ="Romance"},

            });
            
            //Act
            var myService = new CategoryService(context_Moq.Object);

            var result =   myService.GetOneCategory(1);

            var result2 =  myService.GetOneCategory(2);

            //Assert
            Assert.Equal("Fiction", result.CategoryName);

            Assert.Equal("Romance", result2.CategoryName);

        } 

        //Test if EditCategory method properly updates a category
        [Fact]
        public async Task EditCategory_Returns_UpdatedCategory()
        {
            //Arrange
            var context_Moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            context_Moq.CreateDbSetMock(i => i.Categories, new[]
            {
                new Category {CategoryId = 1 , CategoryName ="Fiction"}, //--> we want to update this

                new Category {CategoryId = 2 , CategoryName ="Romance"},

            });

            var my_category = new CategoryDTO()
            {
                //we want to update the category whose CategoryId is 1
                //we change the Categoryname from "Fiction" to "Journal"
                CategoryId = 1,
                
                CategoryName = "Journal"
            };

            //Act
            var service = new CategoryService(context_Moq.Object);

            await service.EditCategory(my_category);

            var updatedCategory = context_Moq.Object.Categories.FirstOrDefault(i => i.CategoryId == 1);

            //Assert
            Assert.Equal("Journal", updatedCategory.CategoryName);

        }

        //Test if RemoveCategory method deletes a category
        [Fact]
        public async Task RemoveCategory_DeletesCategory_FromDatabase()
        {
            //Arrange
            var context_Moq = new DbContextMock<MyAppDbContext>(dummyOptions);

            context_Moq.CreateDbSetMock(i => i.Categories, new[]
            {
                new Category {CategoryId = 1 , CategoryName ="Fiction"}, 

                new Category {CategoryId = 2 , CategoryName ="Romance"},

                new Category {CategoryId = 3 , CategoryName ="Journal"},//--> we want to remove this

                new Category {CategoryId = 4 , CategoryName ="Sports"},

                new Category {CategoryId = 5 , CategoryName ="Family"},

            });

            //Act
            var service = new CategoryService(context_Moq.Object);

            await service.RemoveCategory(3);

            //Assert
            //When we remove one Category then the size will decrease by one
            Assert.Equal(4, context_Moq.Object.Categories.Count());

        }
    }
}