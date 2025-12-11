using Library.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Domain.Ports.Out
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<IEnumerable<Loan>> GetActiveLoansAsync();
        Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId);
    }
}
