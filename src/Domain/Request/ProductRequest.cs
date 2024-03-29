using Domain.Entities;

namespace Domain.Request;

public record struct ProductRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Category Category { get; set; }
}