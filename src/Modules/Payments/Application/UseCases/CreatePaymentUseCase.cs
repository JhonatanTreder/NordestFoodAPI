using NordesteFoodAPI.Modules.Orders.Domain.Contracts;
using NordesteFoodAPI.Modules.Orders.Domain.Enums;
using NordesteFoodAPI.Modules.Payments.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Payments.Domain.Contracts.Services;
using NordesteFoodAPI.Modules.Payments.Domain.DTOs;
using NordesteFoodAPI.Modules.Payments.Domain.Entities;
using NordesteFoodAPI.Modules.Payments.Domain.Enums;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Payments.Application.UseCases
{
    public class CreatePaymentUseCase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CreatePaymentUseCase(IPaymentRepository paymentRepository, IPaymentService paymentService, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _paymentService = paymentService;
            _orderRepository = orderRepository;
        }

        public async Task<Result<PaymentResponseDTO>> CreateAsync(CreatePaymentRequestDTO createPaymentRequestDTO)
        {
            var isSuccessProviderConversion = Enum.TryParse<PaymentProvider>(createPaymentRequestDTO.PaymentProvider, true, out var paymentProvider);

            if (!isSuccessProviderConversion)
            {
                return Result<PaymentResponseDTO>.Failure(
                    $"Não foi possível realizar a conversão do provedor de pagamento '{createPaymentRequestDTO.PaymentProvider}'",
                    ErrorType.ValidationError
                );
            }

            var isSuccessPaymentMethodConversion = Enum.TryParse<PaymentMethod>(createPaymentRequestDTO.PaymentMethod, true, out var paymentMethod);

            if (!isSuccessPaymentMethodConversion)
            {
                return Result<PaymentResponseDTO>.Failure(
                    $"Não foi possível realizar a conversão da forma de pagamento '{createPaymentRequestDTO.PaymentMethod}'",
                    ErrorType.ValidationError
                );
            }

            var orderResponse = await _orderRepository.FindByIdAsync(createPaymentRequestDTO.OrderId);

            if (!orderResponse.IsSuccess)
            {
                return Result<PaymentResponseDTO>.Failure(
                    orderResponse.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar buscar pelo pedido de Id '{createPaymentRequestDTO.OrderId}'",
                    orderResponse.ErrorType
                );
            }

            var order = orderResponse.Value!;

            if (order.OrderStatus != OrderStatus.AguardandoPagamento)
            {
                return Result<PaymentResponseDTO>.Failure(
                    $"O pagamento do pedido de Id '{order.Id}' já foi processado e não pode receber um novo pagamento",
                    ErrorType.ValidationError
                );
            }

            var existingPayment = await _paymentRepository.FindByOrderIdAsync(createPaymentRequestDTO.OrderId);

            if (existingPayment.IsSuccess)
            {
                return Result<PaymentResponseDTO>.Failure(
                    $"O pedido de Id '{createPaymentRequestDTO.OrderId}' já possui um pagamento associado",
                    ErrorType.Conflict
                );
            }

            var processPaymentResponse = await _paymentService.ProcessAsync(createPaymentRequestDTO);

            if (!processPaymentResponse.IsSuccess)
            {
                return Result<PaymentResponseDTO>.Failure(
                    processPaymentResponse.ErrorMessage ?? "Ocorreu um erro inesperado ao processar o pagamento",
                    processPaymentResponse.ErrorType
                );
            }

            var payment = Payment.Create(
                createPaymentRequestDTO.OrderId,
                paymentMethod,
                paymentProvider
            );

            var existingTransactionRef = await _paymentRepository.FindByTransactionRefAsync(payment.TransactionRef);
            if (existingTransactionRef.IsSuccess)
            {
                return Result<PaymentResponseDTO>.Failure(
                    $"A transação de referência '{payment.TransactionRef}' já existe",
                    ErrorType.Conflict
                );
            }

            if (processPaymentResponse.Value == PaymentStatus.Approved)
            {
                payment.MarkAsProcessed();
                order.ConfirmPayment();

                await _orderRepository.UpdateAsync(order);
            }

            var createPaymentResponse = await _paymentRepository.CreateAsync(payment);
            if (!createPaymentResponse.IsSuccess)
            {
                return Result<PaymentResponseDTO>.Failure(
                    createPaymentResponse.ErrorMessage ?? "Ocorreu um erro inesperado ao tentar criar o pagamento",
                    createPaymentResponse.ErrorType
                );
            }

            return Result<PaymentResponseDTO>.Success(new PaymentResponseDTO(
                PaymentId: payment.Id,
                OrderId: payment.OrderId,
                PaymentStatus: payment.PaymentStatus.ToString(),
                PaymentMethod: payment.PaymentMethod.ToString(),
                PaymentProvider: payment.PaymentProvider.ToString(),
                TransactionRef: payment.TransactionRef,
                RequestedAt: payment.RequestedAt,
                ProcessedAt: payment.ProcessedAt
            ));
        }
    }
}
