using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Persistence.Configurations
{
    public class ArticuloBajaConfiguration : IEntityTypeConfiguration<ArticuloBaja>
    {
        public void Configure(EntityTypeBuilder<ArticuloBaja> builder)
        {
            builder.ToTable("tb_articulos_baja");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.BookId).IsRequired().HasColumnType("int");
            builder.Property(x => x.Reason).HasMaxLength(250).HasColumnType("varchar(250)");
            builder.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime(6)");
            builder.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
