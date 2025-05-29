namespace ECommerce.Application.DTOs.BalanceManagement.Order;

public class CreateOrderReqDto
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}