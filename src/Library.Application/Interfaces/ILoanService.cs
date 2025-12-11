using Library.Application.DTOs.Loan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanDto>> GetActiveLoansAsync();
        Task<LoanDto> CreateLoanAsync(CreateLoanDto dto);
        Task ReturnLoanAsync(int loanId);
    }
}
