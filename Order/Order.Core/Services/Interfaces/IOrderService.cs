using Order.Core.Services.Dtos;

namespace Order.Core.Services.Interfaces;

public interface IOrderService
{

    public Task<Entities.Order[]> GetAllAsync();
    public Task<Entities.Order?> GetByIdAsync(Guid id);

    public Task<Entities.Order?> CreateAsync(OrderDto orderDto);
    public Task<Entities.Order?> UpdateItemsAsync(Guid id,  IEnumerable<ItemOrderDto> itemOrder);
    
    public Task<bool> PayOrderAsync(Guid id, string card);
    public Task<bool> ConfirmPayment(Guid id, bool success, string error = "" );
    public Task<bool> DeleteAsync(Guid id);

}