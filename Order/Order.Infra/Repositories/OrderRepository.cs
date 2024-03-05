using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Order.Core.Repositories;
using Shared.Infra;

namespace Order.Infra.Repositories;

public class OrderRepository(OrderContext context) : SharedRepository<Core.Entities.Order>(context), IOrderRepository
{
    private readonly IQueryable<Core.Entities.Order> _base = context.Orders.Include(x => x.Items);

    public override Task<Core.Entities.Order[]> GetAllAsync(bool withDeleted = false)
    {
        var query = _base.AsNoTracking();
        if (!withDeleted)
            query = query.Where(x => x.DeletedAt == null);

        return query.ToArrayAsync();
    }

    public override Task<Core.Entities.Order?> GetByIdAsync(Guid id) =>
        _base.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public Task AttachAsync(Core.Entities.Order order)
    {
        context.Attach(order);
        return Task.CompletedTask;
    }


    public override Task<Core.Entities.Order?> GetByIdTrackingAsync(Guid id) =>
        _base.FirstOrDefaultAsync(x => x.Id == id);
}