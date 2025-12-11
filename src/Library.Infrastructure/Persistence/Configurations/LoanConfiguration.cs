using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Persistence.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StudentName)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");
            builder.Property(x => x.LoanDate)
                .IsRequired()
                .HasColumnType("datetime(6)");
            builder.Property(x => x.ReturnDate)
                .HasColumnType("datetime(6)");
            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime(6)");
            builder.HasOne(x => x.Book).WithMany(b => b.Loans).HasForeignKey(x => x.BookId);
        }
    }
}
