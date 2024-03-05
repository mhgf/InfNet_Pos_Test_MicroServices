namespace Product.Core.Services.Interfaces;

public interface IProductServices
{
    public Task<IEnumerable<Entities.ProductEntity>> GetAllAsync(bool withDelete = false);
    public Task<Entities.ProductEntity?> GetByIdAsync(Guid id);

    public Task<Entities.ProductEntity?> CreateAsync(string description, string category, int value, int initStock);
    public Task<Entities.ProductEntity?> UpdateAsync(Guid id, string description, string category);
    public Task<bool> AddInStock(Guid id, int quantity);
    public Task<bool> RemoveInStock(Guid id, int quantity);
    public Task<bool> DeleteAsync(Guid id);
}