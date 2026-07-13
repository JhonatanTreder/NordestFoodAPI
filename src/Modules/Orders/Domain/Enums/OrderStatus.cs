namespace NordesteFoodAPI.Modules.Orders.Domain.Enums
{
    public enum OrderStatus
    {
        AguardandoPagamento = 0,
        PagamentoConfirmado = 1,
        EmPreparo = 2,
        Pronto = 3,
        Entregue = 4,
        Cancelado = 5
    }
}
