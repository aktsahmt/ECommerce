using ECommerce.Application.Common;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Interfaces;
public interface IOrderService
{
    Task<ServiceResult<OrderHeaderDto?>> GetOrderByIdAsync(Guid orderId);
    Task<ServiceResult<IEnumerable<OrderHeaderDto>>> GetAllOrdersAsync();
    Task<ServiceResult<CreateResultDto>> CreateOrderAsync(CreateDto createDto);
    Task<ServiceResult<CompleteResultDto>> CompleteOrderAsync(CompleteDto completeDto);
    Task<ServiceResult<CancelResultDto>> CancelOrderAsync(CancelDto cancelDto);
}
