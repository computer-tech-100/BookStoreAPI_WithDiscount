using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Contexts;
using MyAPI.Core.Models.DbEntities;
using MyAPI.Core.Models.DTOs;

namespace MyAPI.Core.Services
{
    public class BookService : IBookService
    {
        private readonly MyAppDbContext _context;

        public BookService(MyAppDbContext context)
        {
            _context = context;
        }
       
        public async Task CreateBook(BookDTO book)
        {
            Book my_Book = new Book()
            {
                //Id will be auto generated
                
                Title = book.Title,

                Author = book.Author,

                CategoryId = book.CategoryId,

                Category = book.Category,

                ISBN = book.ISBN,

                Price = book.Price,

                DateOfPublication = book.DateOfPublication
                
            };

            _context.Books.Add(my_Book);

            await _context.SaveChangesAsync();

            await _context.Books.Include(i => i.Category).ToListAsync();

            book.Id = my_Book.Id;

            book.Category = my_Book.Category;

        }

        public async Task<List<BookDTO>> GetAllBooks()
        {
            return (await _context.Books.Include(x => x.Category).ToListAsync()).Select(i => new BookDTO
            {
                Id = i.Id,

                Title = i.Title,

                Author = i.Author,

                CategoryId = i.CategoryId,

                Category = i.Category,

                ISBN = i.ISBN,

                Price = i.Price,

                DateOfPublication = i.DateOfPublication

            }).ToList();

        }

        public BookDTO GetOneBook(int id)
        {
            return _context.Books.Include(x => x.Category).ToList().Select(i => new BookDTO
            {
                Id = i.Id,

                Title = i.Title,

                Author = i.Author,

                CategoryId = i.CategoryId,

                Category = i.Category,

                ISBN = i.ISBN,

                Price = i.Price,

                DateOfPublication = i.DateOfPublication

            }).FirstOrDefault(x => x.Id == id);

        }

        public async Task EditBook(BookDTO book)
        {
            Book chosenBook = _context.Books.Include(c => c.Category).Single(b => b.Id == book.Id);
               
            //update the properties
            chosenBook.Title = book.Title;

            chosenBook.Author = book.Author;

            chosenBook.CategoryId = book.CategoryId;

            chosenBook.ISBN = book.ISBN;

            chosenBook.Price = book.Price;

            chosenBook.DateOfPublication = book.DateOfPublication;

            book.Category = chosenBook.Category;

            await _context.SaveChangesAsync();

        }

        public async Task RemoveBook(int? id)
        {
            Book bookToBeDeleted = _context.Books.Single(b => b.Id ==id);

            _context.Books.Remove(bookToBeDeleted);//remove book from database

            await _context.SaveChangesAsync();
            
        }
    }
    public interface IBookService
    {
        Task CreateBook(BookDTO book);//Post

        Task <List<BookDTO>> GetAllBooks();//Get

        BookDTO GetOneBook(int id);//Get by id

        Task EditBook(BookDTO book);//Put

        Task RemoveBook(int? id);//Delete

    }
}