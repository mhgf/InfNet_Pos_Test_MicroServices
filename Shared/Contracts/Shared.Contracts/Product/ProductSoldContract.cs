namespace Shared.Contracts.Product;

public class ProductSoldContract
{
    public List<ProductItemsContract> Items { get; set; } = [];
}

public class ProductItemsContract
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}