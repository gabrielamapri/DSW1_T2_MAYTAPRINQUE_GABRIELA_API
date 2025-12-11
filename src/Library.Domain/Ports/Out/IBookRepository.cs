using Library.Domain.Entities;
using System.Threading.Tasks;

namespace Library.Domain.Ports.Out
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByISBNAsync(string isbn);
    }
}
