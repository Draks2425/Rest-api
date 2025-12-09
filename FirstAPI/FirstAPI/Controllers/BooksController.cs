using FirstAPI.Models;
using FirstAPI.Repositories; // We need to reference the Repositories namespace
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // Instead of DbContext, we now depend on the Interface
        private readonly IBookRepository _bookRepository;

        // Constructor Injection: Asking for IBookRepository
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            // Using the repository to fetch data
            var books = await _bookRepository.Get();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookRepository.GetById(id);

            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Invalid book data.");
            }

            // The repository handles the adding logic
            var createdBook = await _bookRepository.Add(newBook);

            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            // 1. Fetch the existing book using repository
            var existingBook = await _bookRepository.GetById(id);

            if (existingBook == null)
            {
                return NotFound();
            }

            // 2. Validate input
            if (updatedBook == null || string.IsNullOrEmpty(updatedBook.Title) || string.IsNullOrEmpty(updatedBook.Author) || updatedBook.YearPublished <= 0)
            {
                return BadRequest("Invalid book data.");
            }

            // 3. Map the changes (update properties of the existing book)
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.YearPublished = updatedBook.YearPublished;

            // 4. Save changes using repository
            await _bookRepository.Update(existingBook);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            // Check if exists
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            // Delete via repository
            await _bookRepository.Delete(id);
            return NoContent();
        }
    }
}