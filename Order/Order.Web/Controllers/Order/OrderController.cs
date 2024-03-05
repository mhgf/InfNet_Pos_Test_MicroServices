using Order.Core.Services.Dtos;
using Order.Core.Services.Interfaces;
using Order.Web.Controllers.Order.Dtos;
using Shared.Core.Notification;

namespace Order.Web.Controllers.Order;

public static class OrderController
{
    public static RouteGroupBuilder ConfigureOrderController(this RouteGroupBuilder app)
    {
        var group = app.MapGroup("orders").WithDescription("Order").WithTags("Order");
        group.MapGet("/", GetOrdersAsync);
        group.MapGet("/{id:guid}", GetOrderAsync);
        group.MapPost("/", CreateAsync);
        group.MapPost("/pay/{id:guid}", PayOrderAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);
        return group;
    }
    
    private static async Task<IResult> GetOrdersAsync(IOrderService orderService)
    {
        var users = await orderService.GetAllAsync();

        return TypedResults.Ok(users);
    }

    private static async Task<IResult> GetOrderAsync(Guid id, IOrderService orderService,
        NotificationContext notificationContext)
    {
        if (Guid.Empty == id)
        {
            notificationContext.AddNotification("User", $"User {id} invalido");
            return TypedResults.BadRequest();
        }

        var user = await orderService.GetByIdAsync(id);

        if (user is not null) return TypedResults.Ok(user);

        notificationContext.AddNotification("User", $"User {id} invalido");
        return TypedResults.BadRequest();
    }

    private static async Task<IResult> CreateAsync(OrderDto dto, IOrderService orderService)
    {
        var order = await orderService.CreateAsync(dto);
        return TypedResults.Created($"/api/orders/{order?.Id}", order);
    }
    private static async Task<IResult> PayOrderAsync(Guid id, PayOrderDto dto, IOrderService orderService)
    {
        var order = await orderService.PayOrderAsync(id, dto.Card);
        return TypedResults.Ok(order);
    }
    private static async Task<IResult> UpdateAsync(Guid id, List<ItemOrderDto> items, IOrderService orderService)
    {
        var order = await orderService.UpdateItemsAsync(id, items);
        return TypedResults.Ok(order);
    }

    private static async Task<IResult> DeleteAsync(Guid id, IOrderService orderService)
    {
        var result = await orderService.DeleteAsync(id);
        return TypedResults.Ok(result);
    }
}