using Microsoft.EntityFrameworkCore;
using Shared.Infra;

namespace Product.Infra;

public class ProductContext(DbContextOptions<ProductContext> options) : DbContext(options), IUnitOfWork
{
    private DbSet<Core.Entities.ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.ProductEntity>()
            .ToTable("Products")
            .HasKey(x => x.Id);

        modelBuilder.Entity<Core.Entities.ProductEntity>()
            .Property(x => x.ValueUnit)
            .HasColumnName("productValue")
            .HasDefaultValue(100);
    }

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
}