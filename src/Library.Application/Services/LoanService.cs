using AutoMapper;
using Library.Application.DTOs.Loan;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Exceptions;
using Library.Domain.Ports.Out;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public LoanService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<LoanDto> CreateLoanAsync(CreateLoanDto dto)
        {
            var book = await _uow.Books.GetByIdAsync(dto.BookId);
            if (book == null) throw new NotFoundException("Libro no encontrado");
            if (book.Stock <= 0) throw new BusinessRuleException("No hay stock disponible");

            var loan = new Loan
            {
                BookId = dto.BookId,
                StudentName = dto.StudentName,
                LoanDate = DateTime.Now,
                Status = "Active"
            };

            await _uow.Loans.AddAsync(loan);
            book.Stock -= 1;
            _uow.Books.Update(book);

            await _uow.SaveChangesAsync();

            // Load Book navigation for mapping
            loan.Book = book;

            return _mapper.Map<LoanDto>(loan);
        }

        public async Task<IEnumerable<LoanDto>> GetActiveLoansAsync()
        {
            var loans = await _uow.Loans.GetActiveLoansAsync();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task ReturnLoanAsync(int loanId)
        {
            var loan = await _uow.Loans.GetByIdAsync(loanId);
            if (loan == null) throw new NotFoundException("Préstamo no encontrado");
            if (loan.Status == "Returned") throw new BusinessRuleException("El préstamo ya fue devuelto");

            loan.ReturnDate = DateTime.Now;
            loan.Status = "Returned";
            _uow.Loans.Update(loan);

            var book = await _uow.Books.GetByIdAsync(loan.BookId);
            if (book != null)
            {
                book.Stock += 1;
                _uow.Books.Update(book);
            }

            await _uow.SaveChangesAsync();
        }
    }
}
