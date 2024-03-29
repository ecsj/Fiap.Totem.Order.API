namespace Domain.Entities;

public record struct AdditionalRequest
{
    public Guid ProductId { get; set; }
    public decimal AdditionalPrice { get; set; }
}