using Domain.Base;

namespace Domain.Entities;

public class Order : Entity, IAggregateRoot
{
    public int OrderCode { get; set; }
    public Client? Client { get;  }
    public Guid? ClientId { get; }

    public Payment Payment { get; set; }

    public DateTime OrderDate { get; }
    public List<OrderProduct> Products { get; } = new List<OrderProduct>();
    public OrderStatus Status { get; set; }
    public string OrderStatusDescription => Status.GetDescription();
    public TimeSpan WaitingTime => DateTime.UtcNow - OrderDate;
    public decimal TotalPrice { get; set; }

    public Order() { }
    public Order(Guid? clientId, List<OrderProduct> products, OrderStatus status, decimal totalPrice)
    {
        ClientId = clientId;
        OrderDate = DateTime.UtcNow;
        Products = products;
        Status = status;
        TotalPrice = totalPrice;
        Payment = new Payment(totalPrice, this);

        Validate();
    }

    public void ChangeStatus(OrderStatus status)
    {
        Status = status;
    }

    public void UpdatePayment(PaymentStatus status, string qrCode)
    {
        Payment.ChangeQrCode(qrCode);
        Payment.ChangeStatus(status);
    }
    public static Order FromOrderRequest(OrderRequest orderRequest)
    {
        var products = new List<OrderProduct>();
        foreach (var orderProductRequest in orderRequest.Products)
        {
            var product = new OrderProduct
            {
                ProductId = orderProductRequest.ProductId,
                Quantity = orderProductRequest.Quantity,
                Total = orderProductRequest.Total,
                Comments = orderProductRequest.Comments
            };

            products.Add(product);
        }

        var order = new Order(orderRequest.ClientId, products, OrderStatus.Pending, orderRequest.Total);

        return order;
    }

    public void Validate()
    {
        if (Products == null || Products.Count == 0)
        {
            throw new Exception("O pedido não contém nenhum produto.");
        }

        foreach (var product in Products)
        {
            if (product.Quantity <= 0)
            {
                throw new Exception($"A quantidade para o produto '{product.ProductId}' deve ser maior que zero.");
            }
        }

        if (TotalPrice <= 0)
        {
            throw new Exception("O preço total do pedido deve ser maior que zero.");
        }
    }
}