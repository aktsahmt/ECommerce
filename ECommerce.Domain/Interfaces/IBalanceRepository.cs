using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;
public interface IBalanceRepository : IRepository<Balance>
{
    Task<Balance?> GetByUserIdAsync(string clientCustomerId);
    Task<Balance> UpsertBalance(Balance balance);
}
