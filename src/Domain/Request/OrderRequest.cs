namespace Domain.Entities;

public record struct OrderRequest
{
    public Guid? ClientId { get; set; }
    public List<OrderProductsRequest> Products { get; set; }
    public decimal Total { get; set; }
}
