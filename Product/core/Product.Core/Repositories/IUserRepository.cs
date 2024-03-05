using Shared.Infra;

namespace Product.Core.Repositories;

public interface IProductRepository : ISharedRepository<Entities.ProductEntity>
{
}