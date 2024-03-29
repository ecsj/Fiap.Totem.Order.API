
using Domain.Entities;

namespace Domain.Request;

public class PaymentRequest
{
    public PaymentRequest(Order order)
    {
        OrderId = order.Id;
        PaymentId = order.Payment.Id;
        Amount = order.TotalPrice;
        Currency = "BRL";
        PaymentDate = DateTime.Now;
    }
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime PaymentDate { get; set; }
}