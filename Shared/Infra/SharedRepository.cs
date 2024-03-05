using Microsoft.EntityFrameworkCore;
using Shared.Core.Entity;

namespace Shared.Infra;

public abstract class SharedRepository<T>(DbContext context) : ISharedRepository<T> where T : BaseEntity
{
    public virtual Task<T[]> GetAllAsync(bool withDeleted = false)
    {
        var query = context.Set<T>().AsNoTracking();
        if (!withDeleted)
            query = query.Where(x => x.DeletedAt == null);

        return query.ToArrayAsync();
    }

    public virtual Task<T?> GetByIdTrackingAsync(Guid id) =>
        context.Set<T>().FirstOrDefaultAsync(x => (x as BaseEntity).Id == id);


    public virtual Task<T?> GetByIdAsync(Guid id) =>
        context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => (x as BaseEntity).Id == id);


    public virtual async Task<bool> CreatedAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        return true;
    }

    public virtual Task<bool> UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        return Task.FromResult(true);
    }

    public virtual Task<bool> DeleteAsync(T entity)
    {
        entity.Delete();
        context.Set<T>().Update(entity);
        return Task.FromResult(true);
    }
}