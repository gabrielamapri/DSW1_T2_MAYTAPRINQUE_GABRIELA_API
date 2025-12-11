using AutoMapper;
using Library.Application.DTOs.Book;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Exceptions;
using Library.Domain.Ports.Out;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto)
        {
            var existing = await _uow.Books.GetByISBNAsync(dto.ISBN);
            if (existing != null) throw new BusinessRuleException("El ISBN debe ser único");

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                Stock = dto.Stock
            };

            await _uow.Books.AddAsync(book);
            await _uow.SaveChangesAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _uow.Books.GetByIdAsync(id);
            if (book == null) throw new NotFoundException("Libro no encontrado");

            // Check for any loan history associated with this book (even if returned)
            var loans = await _uow.Loans.GetByBookIdAsync(id);
            if (loans != null && System.Linq.Enumerable.Any(loans))
            {
                throw new BusinessRuleException("No se puede eliminar: existe historial de préstamos asociado a este libro.");
            }

            _uow.Books.Delete(book);
            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var list = await _uow.Books.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(list);
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _uow.Books.GetByIdAsync(id);
            return book == null ? null : _mapper.Map<BookDto>(book);
        }

        public async Task UpdateAsync(int id, UpdateBookDto dto)
        {
            var book = await _uow.Books.GetByIdAsync(id);
            if (book == null) throw new NotFoundException("Libro no encontrado");

            // If ISBN changed, ensure uniqueness
            if (book.ISBN != dto.ISBN)
            {
                var existing = await _uow.Books.GetByISBNAsync(dto.ISBN);
                if (existing != null) throw new BusinessRuleException("El ISBN debe ser único");
            }

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.ISBN = dto.ISBN;
            book.Stock = dto.Stock;

            _uow.Books.Update(book);
            await _uow.SaveChangesAsync();
        }

        public async Task<bool> DarBajaAsync(int id, DTOs.Book.DarBajaDto dto)
        {
            var book = await _uow.Books.GetByIdAsync(id);
            if (book == null) throw new Library.Domain.Exceptions.NotFoundException("Libro no encontrado");

            await _uow.BeginTransactionAsync();
            try
            {
                // Normalize reason: Swagger UI often sends the example literal "string"
                // when users don't change the example value. Treat that as no reason provided.
                var reason = dto?.Reason;
                if (!string.IsNullOrWhiteSpace(reason) && reason.Trim().ToLowerInvariant() == "string")
                {
                    reason = null;
                }

                var baja = new Library.Domain.Entities.ArticuloBaja
                {
                    BookId = id,
                    Reason = reason
                };
                await _uow.Bajas.AddAsync(baja);

                var createdLiquidation = false;
                if (book.Stock > 0)
                {
                    var liq = new Library.Domain.Entities.ArticuloLiquidacion
                    {
                        BookId = id,
                        Quantity = book.Stock
                    };
                    await _uow.Liquidaciones.AddAsync(liq);
                    // set stock to zero since we liquidate existing stock
                    book.Stock = 0;
                    createdLiquidation = true;
                }

                book.IsActive = false;
                _uow.Books.Update(book);
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();
                return createdLiquidation;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
