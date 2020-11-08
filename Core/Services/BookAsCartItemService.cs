using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Contexts;
using MyAPI.Core.Models.DbEntities;
using MyAPI.Core.Models.DTOs;

namespace MyAPI.Core.Services
{
    public class BookAsCartItemService : IBookAsCartItemService
    {
        private readonly MyAppDbContext _context;

        public BookAsCartItemService(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateCartItem(BookAsCartItemDTO favoriteBook)
        {
            BookAsCartItem chosenBook = new BookAsCartItem()
            {
                Id = favoriteBook.Id,

                Book = favoriteBook.Book,

                Price = favoriteBook.Price,

                Quantity = favoriteBook.Quantity,

            };

            _context.BooksAsCartItems.Add(chosenBook);

            await _context.SaveChangesAsync();

            await _context.BooksAsCartItems.Include (i => i.Book).Include(c => c.Book.Category).ToListAsync();

            favoriteBook.Id = chosenBook.Id;

            favoriteBook.Book = chosenBook.Book;//Book will not be null in postman
  
        }

        public async Task<List<BookAsCartItemDTO>> GetAllCartItems()
        {
            return (await _context.BooksAsCartItems.Include(b => b.Book).Include(x => x.Book.Category).ToListAsync()).Select(y => new BookAsCartItemDTO
            {
                Id = y.Id,

                Price = y.Price,

                Book = y.Book,

                Quantity = y.Quantity

            }).ToList();
        }

        public async Task<BookAsCartItemDTO> GetOneCartItem(int id)
        {
            return (await _context.BooksAsCartItems.Include(b => b.Book).Include(x => x.Book.Category).ToListAsync()).Select(y => new BookAsCartItemDTO
            {
                Id = y.Id,

                Price = y.Price,

                Book = y.Book,

                Quantity = y.Quantity

            }).FirstOrDefault(i => i.Id == id);
        }

        public async Task <BookAsCartItemDTO> EditCartItem(BookAsCartItemDTO favoriteBook)
        {
            var favoriteBookToBeUpdated = _context.BooksAsCartItems.Include(b => b.Book).Include(x => x.Book.Category).Single(i =>i.Id == favoriteBook.Id);

            favoriteBookToBeUpdated.Quantity = favoriteBook.Quantity;//update Qauntity

            favoriteBook.Book = favoriteBookToBeUpdated.Book;

            await _context.SaveChangesAsync();

            return favoriteBook;

        }

        public async Task RemoveCartItem(int? id)
        {
            var cartItemToBeDeleted = _context.BooksAsCartItems.Single(i =>i.Id == id);

            _context.BooksAsCartItems.Remove(cartItemToBeDeleted);

            await _context.SaveChangesAsync();
        }
            
    }

    public interface IBookAsCartItemService
    {
        Task CreateCartItem(BookAsCartItemDTO favoriteBook);//Post

        Task<List<BookAsCartItemDTO>> GetAllCartItems();//Get

        Task<BookAsCartItemDTO> GetOneCartItem(int id);//Get by Id

        Task <BookAsCartItemDTO> EditCartItem(BookAsCartItemDTO favoriteBook);//Put

        Task RemoveCartItem(int? id);//Delete
        
    }
}