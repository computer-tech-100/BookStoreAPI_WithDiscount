using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Services;

namespace MyAPI.WebApi.Controllers
{
    [Route("api/[controller]")]//i.e api/Favorites
    [ApiController]//Api Controller
    public class FavoritesController : Controller
    {
        private readonly IFavoritesService _service;

        public FavoritesController (IFavoritesService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task <ActionResult> PostACartItem(FavoritesDTO favoriteBook)
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
        public async Task <ActionResult<List<FavoritesDTO>>> GetListContainingAllCartItems()
        {
            return await _service.GetAllCartItems();
        }

        [HttpGet("{id}")]
        public async Task <ActionResult<FavoritesDTO>> GetCartItemById(int id)
        {
            if(id <= 0)
            {
                return NotFound();
            }

            return await _service.GetOneCartItem(id);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FavoritesDTO>> UpdateCartItem(FavoritesDTO favoriteBook)
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