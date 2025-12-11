using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.ISBN).IsUnique();
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("varchar(200)");
            builder.Property(x => x.Author)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");
            builder.Property(x => x.ISBN)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");
            builder.Property(x => x.Stock)
                .IsRequired()
                .HasColumnType("int");
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime(6)");
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasColumnType("tinyint(1)");
        }
    }
}
