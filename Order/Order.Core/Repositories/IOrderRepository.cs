using Shared.Infra;

namespace Order.Core.Repositories;

public interface IOrderRepository : ISharedRepository<Entities.Order>
{
    Task AttachAsync(Entities.Order id);
}