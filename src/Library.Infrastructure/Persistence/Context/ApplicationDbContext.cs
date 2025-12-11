using Library.Domain.Entities;
using Library.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Loan> Loans { get; set; } = null!;
        public DbSet<Library.Domain.Entities.ArticuloBaja> ArticulosBaja { get; set; } = null!;
        public DbSet<Library.Domain.Entities.ArticuloLiquidacion> ArticulosLiquidacion { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new LoanConfiguration());
            modelBuilder.ApplyConfiguration(new ArticuloBajaConfiguration());
            modelBuilder.ApplyConfiguration(new ArticuloLiquidacionConfiguration());
        }
    }
}
