using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using Order.Core.Enums;
using Shared.Core.Entity;

namespace Order.Core.Entities;

public class Order : BaseEntity
{
    private List<OrderItem> _items = [];
    [JsonPropertyOrder(1)]
    public Guid UserId { get; private set; }

    [JsonPropertyOrder(2)] public OrderStatus Status { get; private set; } = OrderStatus.Open;
    [JsonPropertyOrder(3)]
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();


    public string? Error { get; private set; } = null;

    private Order(Guid userId)
    {
        UserId = userId;
    }

    public static (Order order, ValidationResult? validationResult) Create(Guid userId)
    {
        var order = new Order(userId);
        return (order, new OrderValidator().Validate(order));
    }

    public bool UpdateItems(IEnumerable<OrderItem> items)
    {
        _items = items.ToList();

        return true;
    } 
    public bool UpdateStatus(OrderStatus status, string? error = null )
    {
        Status = status;
        Error = error;
        return true;
    }
}

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(order => order.UserId).NotEqual(Guid.Empty).WithMessage("Produto Invalido");
    }
}