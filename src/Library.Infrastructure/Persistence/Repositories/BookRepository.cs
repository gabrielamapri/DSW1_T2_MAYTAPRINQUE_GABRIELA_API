using Library.Domain.Entities;
using Library.Domain.Ports.Out;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistence.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(DbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Set<Book>().Where(b => b.IsActive).ToListAsync();
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _context.Set<Book>().FirstOrDefaultAsync(b => b.ISBN == isbn);
        }
    }
}
