namespace Order.Core.Services.Dtos;

public class OrderDto
{
    public Guid UserId { get; set; }
    public IList<ItemOrderDto> Items { get; set; } = [];
}

public class ItemOrderDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}