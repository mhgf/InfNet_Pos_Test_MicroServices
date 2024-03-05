using Order.Core.Bus;
using Order.Core.Entities;
using Order.Core.Enums;
using Order.Core.Repositories;
using Order.Core.Services.Dtos;
using Order.Core.Services.Http;
using Order.Core.Services.Interfaces;
using Shared.Contracts.Payment;
using Shared.Contracts.Product;
using Shared.Core.Notification;
using Shared.Infra;

namespace Order.Core.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    NotificationContext notificationContext,
    ISenderProduct senderProduct,
    ISenderPayment senderPayment,
    IProductService productService) : IOrderService
{
    public Task<Entities.Order[]> GetAllAsync() => orderRepository.GetAllAsync();

    public Task<Entities.Order?> GetByIdAsync(Guid id) => orderRepository.GetByIdAsync(id);

    public async Task<Entities.Order?> CreateAsync(OrderDto orderDto)
    {
        var (order, errors) = Entities.Order.Create(orderDto.UserId);
        if (errors is { IsValid: false })
        {
            notificationContext.AddNotifications(errors);
            return null;
        }

        var items = BuildOrderItems(order.Id, orderDto.Items);
        if (items is null) return null;

        order.UpdateItems(items);

        await orderRepository.CreatedAsync(order);
        await unitOfWork.SaveChangesAsync();
        return order;
    }

    public async Task<Entities.Order?> UpdateItemsAsync(Guid id, IEnumerable<ItemOrderDto> itemOrder)
    {
        var order = await orderRepository.GetByIdAsync(id);
        if (order is null)
        {
            notificationContext.AddNotification("Order", "Não encontramos o pedido");
            return null;
        }

        await orderRepository.AttachAsync(order);
        var items = BuildOrderItems(order.Id, itemOrder);
        if (items is null) return null;

        order.UpdateItems(items);
        await unitOfWork.SaveChangesAsync();
        return order;
    }

    public async Task<bool> PayOrderAsync(Guid id, string card)
    {
        var order = await orderRepository.GetByIdAsync(id);
        if (order is null or { Items.Count: 0 })
        {
            notificationContext.AddNotification("Order", "Order Não enctrado ou imcompleta");
            return false;
        }

        await orderRepository.AttachAsync(order);
        try
        {
            order.UpdateStatus(OrderStatus.Pending);

            var products = await productService.GetProducts(order.Items.Select(x => x.ProductId));

            await senderPayment.Send(new MakePaymentContract()
            {
                Card = card,
                OrderId = order.Id,
                Value = products.Sum(x => x.ValueUnit)
            });

            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> ConfirmPayment(Guid id, bool success, string error = "")
    {
        var order = await orderRepository.GetByIdAsync(id);
        if (order is null or { Items.Count: 0 })
        {
            notificationContext.AddNotification("Order", "Order Não enctrado ou imcompleta");
            return false;
        }

        await orderRepository.AttachAsync(order);
        try
        {
            order.UpdateStatus(OrderStatus.Close);


            ProductSoldContract msg = new()
            {
                Items = order.Items.Select(x => new ProductItemsContract()
                {
                    Quantity = x.Quantity,
                    ProductId = x.ProductId
                }).ToList()
            };
            await senderProduct.Send(msg);

            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await orderRepository.GetByIdAsync(id);
        if (order is null)
        {
            notificationContext.AddNotification("Order", "Não encontramos o pedido");
            return false;
        }

        await orderRepository.DeleteAsync(order);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    private List<OrderItem>? BuildOrderItems(Guid orderId, IEnumerable<ItemOrderDto> list)
    {
        var items = new List<OrderItem>();
        foreach (var itemDto in list)
        {
            var (item, error) = OrderItem.Create(orderId, itemDto.ProductId, itemDto.Quantity);
            if (error is { IsValid: false })
            {
                notificationContext.AddNotifications(error);
                return null;
            }

            items.Add(item);
        }

        return items;
    }
}