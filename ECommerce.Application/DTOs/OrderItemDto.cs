namespace ECommerce.Application.DTOs;
public class OrderItemDto
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}