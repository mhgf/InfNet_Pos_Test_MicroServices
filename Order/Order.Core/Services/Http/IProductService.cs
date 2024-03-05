using Order.Core.Services.Dtos;

namespace Order.Core.Services.Http;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProducts(IEnumerable<Guid> ids);
}