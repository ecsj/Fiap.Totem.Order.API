namespace Domain.Entities;

public record OrderProductsRequest
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public string Comments { get; set; }
    public List<AdditionalRequest> Additional { get; set; } = new List<AdditionalRequest>();

}
