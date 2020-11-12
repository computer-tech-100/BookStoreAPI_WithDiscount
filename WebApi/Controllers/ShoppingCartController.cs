using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Services;

namespace MyAPI.WebApi.Controllers
{
    [Route("api/[controller]")]//i.e api/ShoppingCart
    [ApiController]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _service;

        public ShoppingCartController(IShoppingCartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task <ActionResult<ShoppingCartDTO>> GetShoppingCart()
        {
            return await _service.GetCart();

        }
    }
}