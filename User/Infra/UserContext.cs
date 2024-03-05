using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Shared.Infra;

namespace Infra;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options), IUnitOfWork
{
    private DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasKey(x => x.Id);
    }

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
}