using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using Shared.Core.Entity;

namespace Order.Core.Entities;

public class OrderItem : BaseEntity
{
    [JsonPropertyOrder(1)]
    public Guid ProductId { get; private set; }
    [JsonPropertyOrder(2)]
    public Guid OrderId { get; private set; }
    [JsonPropertyOrder(3)]
    public int Quantity { get; private set; }
    [JsonIgnore]
    public Order? Order { get; private set; } = null;
    private OrderItem(Guid orderId,Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public static (OrderItem orderItem, ValidationResult? errors) Create(Guid orderId,Guid productId, int quantity)
    {
        var orderItem = new OrderItem(orderId, productId, quantity);
        return (orderItem, new OrderItemValidator().Validate(orderItem));
    }

    public void UpdateQuantity(int quantity) => Quantity = quantity;
}


public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(item => item.ProductId).NotEqual(Guid.Empty).WithMessage("Produto Invalido");
        RuleFor(item => item.Quantity).GreaterThanOrEqualTo(1).WithMessage("Quantidade invalida");
    }
}