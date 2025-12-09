using FirstAPI.Models;

namespace FirstAPI.Repositories
{
    // Interface defining the operations for Book data access
    public interface IBookRepository
    {
        // Get all books
        Task<IEnumerable<Book>> Get();

        // Get a specific book by its ID
        Task<Book?> GetById(int id);

        // Add a new book to the database
        Task<Book> Add(Book book);

        // Update an existing book
        Task Update(Book book);

        // Delete a book by its ID
        Task Delete(int id);
    }
}