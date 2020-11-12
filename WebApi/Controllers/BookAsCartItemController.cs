using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Services;

namespace MyAPI.WebApi.Controllers
{
    [Route("api/[controller]")]//i.e api/BookAsCartItem
    [ApiController]//Api Controller
    public class BookAsCartItemController : Controller
    {
        private readonly IBookAsCartItemService _service;

        public BookAsCartItemController(IBookAsCartItemService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task <ActionResult> PostACartItem(BookAsCartItemDTO favoriteBook)
        {
            if(favoriteBook == null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _service.CreateCartItem(favoriteBook);

            return Ok(favoriteBook);

        }

        [HttpGet]
        public async Task <ActionResult<List<BookAsCartItemDTO>>> GetListContainingAllCartItems()
        {
            return await _service.GetAllCartItems();
        }

        [HttpGet("{id}")]
        public ActionResult<BookAsCartItemDTO> GetCartItemById(int id)
        {
            if(id <= 0)
            {
                return NotFound();
            }

            return Ok(_service.GetOneCartItem(id));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCartItem(BookAsCartItemDTO favoriteBook)
        {
            if(favoriteBook == null)
            {
                return NotFound();
            }

            await _service.EditCartItem(favoriteBook);

            return Ok(favoriteBook);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItem(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            await _service.RemoveCartItem(id);

            return Ok();
        }
    }
}