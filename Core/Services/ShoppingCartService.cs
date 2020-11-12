using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Contexts;
using MyAPI.Core.Models.DTOs;

namespace MyAPI.Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly MyAppDbContext _context;

        public ShoppingCartService(MyAppDbContext context)
        {
            _context = context;
        } 

        public async Task<ShoppingCartDTO> GetCart()
        {
            var AllCartItems = (await _context.BooksAsCartItems.Include(i => i.Book).Include(c => c.Book.Category).ToListAsync()).Select(t => new BookAsCartItemDTO
            {
                Id = t.Id,

                Price = t.Price,

                Book = t.Book,

                Quantity = t.Quantity


            }).ToList();

            ShoppingCartDTO shoppingCart = new ShoppingCartDTO()
            {
                AllBooksInsideCart = AllCartItems,

                Total = AllCartItems.Sum(p => p.Price * p.Quantity)//calculate total bill

            };

            //when total amount is above 100 dollors then we give 10% discount i.e GrandTotal = Total * 0.1
            //otherwise GrandTotal = Total
            if(shoppingCart.Total > 100)
            {
                shoppingCart.Discount = shoppingCart.Total * 10/100;

                shoppingCart.GrandTotal = shoppingCart.Total - shoppingCart.Discount;
            }
            else
            {
                shoppingCart.Discount = 0;

                shoppingCart.GrandTotal = shoppingCart.Total;

            }
            
            return shoppingCart;
        }
        
    }

    public interface IShoppingCartService
    {
        Task<ShoppingCartDTO> GetCart();
        
    }
}