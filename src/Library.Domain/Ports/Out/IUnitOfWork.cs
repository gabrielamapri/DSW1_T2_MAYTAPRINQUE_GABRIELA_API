namespace Library.Domain.Ports.Out
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        ILoanRepository Loans { get; }
        IArticuloBajaRepository Bajas { get; }
        IArticuloLiquidacionRepository Liquidaciones { get; }

        System.Threading.Tasks.Task<int> SaveChangesAsync();

        // Transaction control
        System.Threading.Tasks.Task BeginTransactionAsync();
        System.Threading.Tasks.Task CommitAsync();
        System.Threading.Tasks.Task RollbackAsync();
    }
}
