using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IOrderRepository : IRepository<OrderHeader>
{
    Task AddOrderAsync(OrderHeader orderHeader);
    Task UpdateOrderStatusByIdAsync(Guid id, string status);
}

