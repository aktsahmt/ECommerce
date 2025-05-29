namespace ECommerce.Domain.Entities;

public class OrderHeader : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; } = null;
    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
