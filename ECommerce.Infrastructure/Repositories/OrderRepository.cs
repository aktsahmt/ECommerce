using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Persistences;

namespace ECommerce.Infrastructure.Repositories;
public class OrderRepository : GenericRepository<OrderHeader>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {

    }

    public async Task AddOrderAsync(OrderHeader orderHeader)
    {
        await AddAsync(orderHeader);

        foreach (var orderLine in orderHeader.OrderLines)
        {
            orderLine.OrderHeaderId = orderHeader.Id;
            await _context.OrderLines.AddAsync(orderLine);
        }
    }

    public async Task UpdateOrderStatusByIdAsync(Guid id, string status)
    {
        var order = await GetByIdAsync(id);
        if (order != null)
        {
            order.Status = status;
            Update(order);
        }
    }
}
