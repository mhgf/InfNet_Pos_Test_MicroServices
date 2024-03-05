using Product.Core.Repositories;
using Shared.Infra;

namespace Product.Infra.Repositories;

public class ProductRepository(ProductContext context) : SharedRepository<Core.Entities.ProductEntity>(context), IProductRepository
{
}