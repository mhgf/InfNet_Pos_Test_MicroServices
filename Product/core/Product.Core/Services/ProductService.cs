using Product.Core.Repositories;
using Product.Core.Services.Interfaces;
using Shared.Core.Notification;
using Shared.Infra;

namespace Product.Core.Services;

public class ProductService(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    NotificationContext notificationContext) : IProductServices
{
    public async Task<IEnumerable<Entities.ProductEntity>> GetAllAsync(bool withDelete = false)
    {
        return await productRepository.GetAllAsync(withDelete);
    }

    public async Task<Entities.ProductEntity?> GetByIdAsync(Guid id)
    {
        return await productRepository.GetByIdAsync(id);
    }

    public async Task<Entities.ProductEntity?> CreateAsync(string description, string category, int value, int initStock)
    {
        var (product, errors) = Entities.ProductEntity.Create(description, category, value, initStock);

        if (errors is { IsValid: false })
        {
            notificationContext.AddNotifications(errors);
            return null;
        }

        await productRepository.CreatedAsync(product);
        await unitOfWork.SaveChangesAsync();
        return product;
    }

    public async Task<Entities.ProductEntity?> UpdateAsync(Guid id, string description, string category)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product is null)
        {
            notificationContext.AddNotification("Product", "Produto não encontrado");
            return null;
        }

        var errors = product.Update(description, category);

        if (errors is { IsValid: false })
        {
            notificationContext.AddNotifications(errors);
            return null;
        }

        await productRepository.UpdateAsync(product);
        await unitOfWork.SaveChangesAsync();
        return product;
    }

    public async Task<bool> AddInStock(Guid id, int quantity)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product is null)
        {
            notificationContext.AddNotification("Product", "Produto não encontrado");
            return false;
        }

        var result =product.AddStock(quantity);
        if (!result) return result;
        
        await productRepository.UpdateAsync(product);
        await unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<bool> RemoveInStock(Guid id, int quantity)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product is null)
        {
            notificationContext.AddNotification("Product", "Produto não encontrado");
            return false;
        }
        
        var result =product.RemoveStock(quantity);
        if (!result) return result;
        
        await productRepository.UpdateAsync(product);
        await unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await productRepository.GetByIdAsync(id);

        if (user is not null) return await productRepository.DeleteAsync(user);

        notificationContext.AddNotification("Product", $"Produto {id} não encontrado");
        return false;
    }
}