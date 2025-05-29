using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;
public class BalanceRepository : GenericRepository<Balance>, IBalanceRepository
{
    public BalanceRepository(OrderDbContext context) : base(context)
    {
    }

    public async Task<Balance?> GetByUserIdAsync(string userId)
    {
        return await _context.Balance.FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Balance> UpsertBalance(Balance balance)
    {
        var existingBalance = await GetByUserIdAsync(balance.UserId);

        if (existingBalance != null)
        {
            existingBalance.TotalBalance = balance.TotalBalance;
            existingBalance.AvailableBalance = balance.AvailableBalance;
            existingBalance.BlockedBalance = balance.BlockedBalance;
            existingBalance.LastUpdated = balance.LastUpdated;
            Update(existingBalance);

            return existingBalance;
        }

        await AddAsync(balance);

        return balance;
    }
}


