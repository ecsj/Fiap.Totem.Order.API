using System.ComponentModel;

namespace Domain.Entities;

public enum OrderStatus
{
    [Description("Recebido")]
    Pending,
    [Description("Em preparaçao")]
    InPreparation,
    [Description("Aguardando Pagamento")]
    PendingPayment,
    [Description("Pagamento Autorizado")]
    AuthorizedPayment,
    [Description("Pagamento recusado")]
    UnauthorizedPayment,
    [Description("Pronto")]
    Completed,
    [Description("Finalizado")]
    Finished,
    [Description("Cancelado")]
    Canceled
}