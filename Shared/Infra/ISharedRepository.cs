namespace Shared.Infra;

public interface ISharedRepository<T>
{
    public Task<T[]> GetAllAsync(bool withDeleted = false);
    public Task<T?> GetByIdTrackingAsync(Guid id);
    public Task<T?> GetByIdAsync(Guid id);
    public Task<bool> CreatedAsync(T entity);
    public Task<bool> UpdateAsync(T entity);
    public Task<bool> DeleteAsync(T entity);
}