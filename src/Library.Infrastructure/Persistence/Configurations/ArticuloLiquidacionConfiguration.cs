using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Persistence.Configurations
{
    public class ArticuloLiquidacionConfiguration : IEntityTypeConfiguration<ArticuloLiquidacion>
    {
        public void Configure(EntityTypeBuilder<ArticuloLiquidacion> builder)
        {
            builder.ToTable("tb_articulos_liquidacion");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.BookId).IsRequired().HasColumnType("int");
            builder.Property(x => x.Quantity).IsRequired().HasColumnType("int");
            builder.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime(6)");
            builder.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
