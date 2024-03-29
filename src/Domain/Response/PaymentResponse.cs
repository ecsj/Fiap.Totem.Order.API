using Domain.Base;
using Domain.Entities;

namespace Domain.Response;

public class PaymentResponse
{
    public Guid Id { get; set; }
    public PaymentStatus Status { get; set; }
    public string? QrCode { get; set; }
    public string PaymentStatusDescription => Status.GetDescription();
    public Guid OrderId { get; set; }
}