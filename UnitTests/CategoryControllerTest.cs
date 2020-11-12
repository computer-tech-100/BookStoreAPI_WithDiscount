using Xunit;
using Moq;
using MyAPI.Core.Services;
using System.Collections.Generic;
using MyAPI.Core.Models.DTOs;
using System.Threading.Tasks;
using MyAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MyAPI.UnitTests
{
    public class CategoryControllerTest
    {
        //create dummy data
        private List<CategoryDTO> DummyCategory()
        {
            List<CategoryDTO> my_List = new List<CategoryDTO>();//create a new list

            //create a new category
            CategoryDTO myNewCategory1 = new CategoryDTO()
            {
                CategoryId = 1,
                CategoryName = "Fiction"
            };

            my_List.Add(myNewCategory1);//add your new category to the list

            //create a new category
            CategoryDTO myNewCategory2 = new CategoryDTO()
            {
                CategoryId = 2,
                CategoryName = "Sports"
            };

            my_List.Add(myNewCategory2);//add your new category to the list

            return my_List;   

        }

        //Test Post Method
        [Fact]
        public async Task PostACategory_WhenValidObjectIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock <ICategoryService>();

            CategoryDTO category = new CategoryDTO()
            {
                CategoryId =1,

                CategoryName = "Fiction" 
            };
            
            //Return will give you some random number for categoryId
            moq.Setup(i => i.CreateCategory(It.IsAny<CategoryDTO>())).Returns(() => Task.FromResult(category));

            //Act
            var controller = new CategoryController(moq.Object);

            var response = await controller.PostACategory(category);

            //Assert
            Assert.IsType<OkObjectResult>(response);

            var created_response = Assert.IsType<OkObjectResult>(response);

            Assert.True( (created_response.Value as CategoryDTO).CategoryId > 0 );
  
        }
         
        //Test Post method when null is passed
        [Fact]
        public async Task PostACategory_WhenNullIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq =new Mock<ICategoryService>();

            moq.Setup(i => i.CreateCategory(null));

            //Act
            var controller = new CategoryController(moq.Object);

            var result = await controller.PostACategory(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
         
        //Test Get method
        [Fact]
        public async Task GetListContainingAllCategories_WhenCalledReturnsAllCategories()
        {
            //Arrange
            var moq = new Mock<ICategoryService>();

            moq.Setup(i => i.GetAllCategories()).ReturnsAsync(DummyCategory());

            //Act
            var controller = new CategoryController(moq.Object);

            var all = await controller.GetListContainingAllCategories();

            //Assert
            Assert.NotNull(all);

        }
        
        //Test Get by id method when invalid object is passed
        [Fact]
        public void GetCategoryById_WhenInValidIdIsPassed_ReturnsNotFoundResult()
        {
            //Arrange
            var moq = new Mock<ICategoryService>();

            moq.Setup(x => x.GetOneCategory(0)).Returns(DummyCategory().FirstOrDefault(i=>i.CategoryId == 0));

            //Act
            var controller = new CategoryController(moq.Object);

            var myResult = controller.GetCategoryById(0);//Id 0 is an invalid id

            //Assert
            Assert.IsType<NotFoundResult>(myResult.Result);

        }

        //Test Get by id method when valid object is passed
        [Fact]
        public void GetCategoryById_WhenValidIdIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<ICategoryService>();

            moq.Setup(x => x.GetOneCategory(2)).Returns(DummyCategory().FirstOrDefault(i=>i.CategoryId == 2));

            //Act
            var controller = new CategoryController(moq.Object);

            var myResult = controller.GetCategoryById(2);//Id 2 is a valid id

            //Assert
            Assert.IsType<OkObjectResult>(myResult.Result);

        }

        //Test get by id method when valid id is passed it should return correct category
        [Fact]
        public void GetCategoryById_WhenValidIdPassed_ReturnsCorrectCategory()
        {
            //Arrange
            var moq = new Mock<ICategoryService>();

            moq.Setup(i => i.GetOneCategory(1)).Returns(DummyCategory().FirstOrDefault(x => x.CategoryId == 1));

            //Act
            var controller = new CategoryController(moq.Object);

            var category = controller.GetCategoryById(1).Result as OkObjectResult;

            //Assert
            Assert.Equal("Fiction", (category.Value as CategoryDTO).CategoryName);
            
        }

        //Test Put method when null is passed
        [Fact]
        public async Task UpdateCategory_WhenNullIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq = new Mock<ICategoryService>();

            moq.Setup(t => t.EditCategory(null));

            //Act
            var controller = new CategoryController(moq.Object);

            var result = await controller.UpdateCategory(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }

        //Test Put method when existing category is passed
        [Fact]
        public async Task UpdateCategory_WhenValidCategoryIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<ICategoryService>();

            CategoryDTO category = new CategoryDTO()
            {
                CategoryId = 3,
                CategoryName ="Non - Fiction"
            };

            moq.Setup(r => r.EditCategory(category));

            //Act
            var controller = new CategoryController(moq.Object);

            var ok_Result = await controller.UpdateCategory(category);

            //Assert
            Assert.IsType <OkObjectResult>(ok_Result);

        }

        //Test Delete method when null is passed
        [Fact]
        public async Task DeleteCategory_WhenInValidIdIsPassed_ReturnsNotFound()
        {
            //Arrange
            var moq = new Mock<ICategoryService>();

            moq.Setup(r => r.RemoveCategory(null));

            //Act
            var controller = new CategoryController(moq.Object);

            var result = await controller.DeleteCategory(null);

            //Assert
            Assert.IsType <NotFoundResult>(result);

        }

        //Test Delete method when valid id is passed
        [Fact]
        public async Task DeleteCategory_WhenValidIdIsPassed_ReturnsOkResult()
        {
            //Arrange
            var moq = new Mock<ICategoryService>();

            moq.Setup(r => r.RemoveCategory(1));

            //Act
            var controller = new CategoryController(moq.Object);

            var result = await controller.DeleteCategory(1);

            //Assert
            Assert.IsType <OkResult>(result);

        }
    }
}