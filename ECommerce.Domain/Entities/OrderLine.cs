namespace ECommerce.Domain.Entities;


public class OrderLine : BaseEntity
{
    public Guid OrderHeaderId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public virtual OrderHeader OrderHeader { get; set; } = null!;
}


