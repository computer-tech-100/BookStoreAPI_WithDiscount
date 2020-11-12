using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Core.Models.DTOs;
using MyAPI.Core.Services;
using System.Collections.Generic;


namespace MyAPI.WebApi.Controllers
{
   [Route("api/[controller]")]//i.e api/Book 
   [ApiController]
    public class BookController : Controller
    {
        private readonly IBookService _sevice;

        public BookController(IBookService service)
        {
            _sevice = service;
        }

        [HttpPost]
        public async Task<ActionResult> PostABook(BookDTO book)
        {
            if (book == null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();

            }

            await _sevice.CreateBook(book);

            return Ok(book);

        }

        [HttpGet]
        public async Task <ActionResult<List<BookDTO>>> GetListContainingAllBooks()
        {
            return await _sevice.GetAllBooks();

        }

        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBookById(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(_sevice.GetOneBook(id));

        }

        [HttpPut("{id}")]
        public async Task <ActionResult> UpdateBook(BookDTO book)
        {
            if(book == null)
            {
                return NotFound();
            }

           await _sevice.EditBook(book);

           return Ok(book);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            await _sevice.RemoveBook(id);

            return Ok();
        }   
    }
}