using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Services;

namespace MyAPI.WebApi.Controllers
{
   [Route("api/[controller]")]//i.e api/Category --> Category is name of our controller
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
        public async Task <ActionResult> PostCategory(CategoryDTO category)
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
        
    }
}
