using Microsoft.EntityFrameworkCore;
using Order.Core.Enums;
using Shared.Infra;

namespace Order.Infra;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Core.Entities.Order> Orders { get; set; }
    public DbSet<Core.Entities.OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.Order>()
            .ToTable("Orders")
            .HasKey(x => x.Id);
        modelBuilder.Entity<Core.Entities.OrderItem>()
            .ToTable("OrderItems")
            .HasKey(x => x.Id);

        modelBuilder.Entity<Core.Entities.Order>()
            .HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade).IsRequired();

        modelBuilder.Entity<Core.Entities.Order>()
            .Property(x => x.Status)
            .HasDefaultValue(OrderStatus.Open);
        modelBuilder.Entity<Core.Entities.Order>()
            .Property(x => x.Error)
            .HasDefaultValue(null);
        
        modelBuilder.Entity<Core.Entities.Order>()
            .Metadata
            .FindNavigation(nameof(Core.Entities.Order.Items))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}