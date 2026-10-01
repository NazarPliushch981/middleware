using middleware.Models;
using Microsoft.AspNetCore.Mvc;
using middleware.Models;

namespace middleware.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static List<Book> books = new List<Book>
        {
            new Book { Id = 1, Title = "Kobzar", Author = "Taras Shevchenko", Year = 1840 },
            new Book { Id = 2, Title = "Misto", Author = "Valerian Pidmohylny", Year = 1928 },
            new Book { Id = 3, Title = "Intermezzo", Author = "Mykhailo Kotsiubynsky", Year = 1908 }
        };
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Book book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            return Ok(book);
        }
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string title, [FromQuery] string author)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("Параметр 'title' є обов'язковим!");
            }

            var result = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(author))
            {
                result = result.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book newBook)
        {
            if (newBook == null || string.IsNullOrEmpty(newBook.Title) || string.IsNullOrEmpty(newBook.Author) || newBook.Year < 1800)
            {
                return BadRequest("Некоректні дані!");
            }

            int newId = 1;
            if (books.Count > 0)
            {
                newId = books.Max(b => b.Id) + 1;
            }

            newBook.Id = newId;
            books.Add(newBook);

            return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Book updatedBook)
        {
            Book book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound("not found");
            }

            if (updatedBook == null || string.IsNullOrEmpty(updatedBook.Title) || string.IsNullOrEmpty(updatedBook.Author) || updatedBook.Year < 1800)
            {
                return BadRequest("Некоректнo");
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Year = updatedBook.Year;

            return Ok(book);
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Book book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound("not found");
            }

            books.Remove(book);

            return NoContent();
        }
    }
}