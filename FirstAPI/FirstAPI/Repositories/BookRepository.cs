using FirstAPI.Data;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Repositories
{
    //implementation of the IBookRepository using Entity Framework Core
    public class BookRepository : IBookRepository
    {
        private readonly FirstAPIContext _context;

        //injecting the DbContext via constructor
        public BookRepository(FirstAPIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> Get()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> Add(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task Update(Book book)
        {
            //assume the book object is already tracked or update its state
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var bookToDelete = await _context.Books.FindAsync(id);
            if (bookToDelete != null)
            {
                _context.Books.Remove(bookToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}