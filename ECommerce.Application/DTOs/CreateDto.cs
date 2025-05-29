namespace ECommerce.Application.DTOs;

public class CreateDto
{
    public Guid UserId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}