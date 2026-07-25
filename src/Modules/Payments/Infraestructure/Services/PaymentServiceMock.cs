using NordesteFoodAPI.Modules.Payments.Domain.Contracts.Services;
using NordesteFoodAPI.Modules.Payments.Domain.DTOs;
using NordesteFoodAPI.Modules.Payments.Domain.Enums;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Payments.Infraestructure.Services
{
    public class PaymentServiceMock : IPaymentService
    {
        public Task<Result<PaymentStatus>> ProcessAsync(CreatePaymentRequestDTO paymentRequestDTO)
        {
            switch (paymentRequestDTO.PaymentProvider)
            {
                //Aqui estou simulando casos diferentes de retorno do provedor de pagamento para ficar mais realista com um pagamento real, justamente por ser um mock para testes.
                case "MercadoPago":
                    return Task.FromResult(Result<PaymentStatus>.Success(PaymentStatus.Approved));

                case "PagSeguro":
                    return Task.FromResult(Result<PaymentStatus>.Success(PaymentStatus.Failed));

                case "Stripe":
                    return Task.FromResult(Result<PaymentStatus>.Success(PaymentStatus.Denied));

                default:
                    return Task.FromResult(Result<PaymentStatus>.Failure(
                        "O provedor de pagamento está inválido. Aceitamos apenas MercadoPago, PagSeguro e Stripe",
                        ErrorType.Failure)
                    );
            }
        }
    }
}
