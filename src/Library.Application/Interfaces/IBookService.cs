using Library.Application.DTOs.Book;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(int id);
        Task<BookDto> CreateAsync(CreateBookDto dto);
        Task UpdateAsync(int id, UpdateBookDto dto);
        Task DeleteAsync(int id);
        Task<bool> DarBajaAsync(int id, DTOs.Book.DarBajaDto dto);
    }
}
