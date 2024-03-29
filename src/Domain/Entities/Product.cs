using Domain.Base;
using Domain.Request;

namespace Domain.Entities;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Category Category { get; set; }

    public static Product FromProductRequest(ProductRequest productRequest)
    {
        return new Product
        {
            Id = productRequest.Id,
            Name = productRequest.Name,
            Price = productRequest.Price,
            Category = productRequest.Category
        };
    }
}