using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.BalanceManagement.Order;

namespace ECommerce.Application.Interfaces;

public interface IClientBalanceService
{
    Task<ServiceResult<List<ProductDto>>> GetProductsAsync();
    Task<PreOrderRootDto> PreOrderAsync(CreateOrderReqDto createOrderReqDto);
    Task<PreOrderRootDto> CompleteAsync(CompleteOrderReqDto completeOrderReqDto);
    Task<PreOrderRootDto> CancelAsync(CancelOrderReqDto cancelOrderReqDto);
}