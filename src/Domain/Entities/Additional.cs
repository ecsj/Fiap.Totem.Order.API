using Domain.Base;

namespace Domain.Entities;

public class Additional : Entity
{
    public Product Product { get; set; }
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }
}
