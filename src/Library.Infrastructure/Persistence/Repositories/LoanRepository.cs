using Library.Domain.Entities;
using Library.Domain.Ports.Out;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistence.Repositories
{
    public class LoanRepository : Repository<Loan>, ILoanRepository
    {
        public LoanRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
        {
            return await _context.Set<Loan>().Include(l => l.Book).Where(l => l.Status == "Active").ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId)
        {
            return await _context.Set<Loan>().Where(l => l.BookId == bookId).ToListAsync();
        }
    }
}
