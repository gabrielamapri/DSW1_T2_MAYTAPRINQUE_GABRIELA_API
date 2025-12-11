using Library.Domain.Ports.Out;
using Library.Infrastructure.Persistence.Repositories;
using Library.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private BookRepository? _bookRepository;
        private LoanRepository? _loanRepository;
        private ArticuloBajaRepository? _bajaRepository;
        private ArticuloLiquidacionRepository? _liquidacionRepository;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _currentTransaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBookRepository Books => _bookRepository ??= new BookRepository(_context);
        public ILoanRepository Loans => _loanRepository ??= new LoanRepository(_context);
        public IArticuloBajaRepository Bajas => _bajaRepository ??= new ArticuloBajaRepository(_context);
        public IArticuloLiquidacionRepository Liquidaciones => _liquidacionRepository ??= new ArticuloLiquidacionRepository(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_currentTransaction == null) return;
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction == null) return;
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
