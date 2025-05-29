using ECommerce.Application.Common;
using ECommerce.Application.DTOs.BalanceManagement.Order;

namespace ECommerce.Application.Interfaces;
public interface IBalanceService
{
    Task<ServiceResult<IEnumerable<BalanceDto>>> GetAllBalanceAsync();
    Task<ServiceResult<BalanceDto>> GetBalanceByUserIdAsync(string userId);
}

