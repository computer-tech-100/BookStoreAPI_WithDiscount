using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Contexts;
using MyAPI.Core.Models.DbEntities;
using MyAPI.Core.Models.DTOs;

namespace MyAPI.Core.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly MyAppDbContext _context;

        public FavoritesService(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateCartItem(FavoritesDTO favoriteBook)
        {
            Favorites chosenBook = new Favorites()
            {
                Id = favoriteBook.Id,

                Book = favoriteBook.Book,

                Price = favoriteBook.Price,

                Quantity = favoriteBook.Quantity,

            };

            _context.CartItems.Add(chosenBook);

            await _context.SaveChangesAsync();

            await _context.CartItems.Include (i => i.Book).Include(c => c.Book.Category).ToListAsync();

            favoriteBook.Id = chosenBook.Id;

            favoriteBook.Book = chosenBook.Book;//Book will not be null in postman
  
        }

        public async Task<List<FavoritesDTO>> GetAllCartItems()
        {
            return (await _context.CartItems.Include(b => b.Book).Include(x => x.Book.Category).ToListAsync()).Select(y => new FavoritesDTO
            {
                Id = y.Id,

                Price = y.Price,

                Book = y.Book,

                Quantity = y.Quantity

            }).ToList();
        }

        public async Task<FavoritesDTO> GetOneCartItem(int id)
        {
            return (await _context.CartItems.Include(b => b.Book).Include(x => x.Book.Category).ToListAsync()).Select(y => new FavoritesDTO
            {
                Id = y.Id,

                Price = y.Price,

                Book = y.Book,

                Quantity = y.Quantity

            }).FirstOrDefault(i => i.Id == id);
        }

        public async Task <FavoritesDTO> EditCartItem(FavoritesDTO favoriteBook)
        {
            var favoriteBookToBeUpdated = _context.CartItems.Include(b => b.Book).Include(x => x.Book.Category).Single(i =>i.Id == favoriteBook.Id);

            favoriteBook.Quantity = favoriteBook.Quantity;//update Qauntity

            favoriteBook.Book = favoriteBookToBeUpdated.Book;

            await _context.SaveChangesAsync();

            return favoriteBook;

        }

        public async Task RemoveCartItem(int? id)
        {
            var cartItemToBeDeleted = _context.CartItems.Single(i =>i.Id == id);

            _context.CartItems.Remove(cartItemToBeDeleted);

            await _context.SaveChangesAsync();
        }
            
    }

    public interface IFavoritesService
    {
        Task CreateCartItem(FavoritesDTO favoriteBook);//Post

        Task<List<FavoritesDTO>> GetAllCartItems();//Get

        Task<FavoritesDTO> GetOneCartItem(int id);//Get by Id

        Task <FavoritesDTO> EditCartItem(FavoritesDTO favoriteBook);//Put

        Task RemoveCartItem(int? id);//Delete
    }
}