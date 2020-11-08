using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Services;

namespace MyAPI.WebApi.Controllers
{
   [Route("api/[controller]")]//i.e api/Category
   [ApiController]//Api Controller
    public class CategoryController : Controller
    {
        //controller always references service
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;

        }

        [HttpPost]
        public async Task <ActionResult> PostACategory(CategoryDTO category)
        {
            if (category == null)
            {
                return NotFound();
            }
            
            //check if Model State is valid or not
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _service.CreateCategory(category);
            
            return Ok(category);

        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetListContainingAllCategories()
        {
            return await _service.GetAllCategories();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return await _service.GetOneCategory(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(CategoryDTO category)
        {
            if (category == null)
            {
                return NotFound();
            }

            await _service.EditCategory(category);

            return Ok(category);

        } 

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
           
            await _service.RemoveCategory(id);
            
            return Ok(); 

        }
    }
}