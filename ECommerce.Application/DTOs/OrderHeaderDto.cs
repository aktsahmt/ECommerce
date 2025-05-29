namespace ECommerce.Application.DTOs;
public class OrderHeaderDto
{
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; } = null;
}