using Domain.Base;
using Domain.Entities;

namespace Domain.Response;

public class OrderResponse
{
    public Guid Id { get; set; }
    public int OrderCode { get; set; }
    public OrderStatus Status { get; private set; }
    public PaymentResponse Payment { get; set; }

    public OrderResponse GenerateResponse(Order order)
    {
        return new OrderResponse()
        {
            Id = order.Id,
            OrderCode = order.OrderCode,
            Status = order.Status,
            Payment = new PaymentResponse()
            {
                Status = order.Payment.Status,
                QrCode = order.Payment.QrCode
            }

        };

    }
    public string OrderStatusDescription => Status.GetDescription();
}