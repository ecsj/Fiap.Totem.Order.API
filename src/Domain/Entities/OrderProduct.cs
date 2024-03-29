namespace Domain.Entities;

public class OrderProduct
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public string ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public string Comments { get; set; }
    public decimal Total { get; set; }
}
