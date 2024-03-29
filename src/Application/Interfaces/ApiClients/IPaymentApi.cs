using Domain.Request;
using Domain.Response;
using Refit;

namespace Application.Interfaces.ApiClients;
public interface IPaymentApi
{
    [Get("/Payment/{id}")]
    Task<PaymentResponse> GetPaymentDetails(int id);

    [Post("/Payment/request")]
    Task<PaymentResponse> ProcessPayment([Body] PaymentRequest payment);
}