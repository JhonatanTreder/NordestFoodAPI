using NordesteFoodAPI.Modules.Feedbacks.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Feedbacks.Domain.DTOs;
using NordesteFoodAPI.Modules.Feedbacks.Domain.Entities;
using NordesteFoodAPI.Modules.Feedbacks.Domain.Enums;
using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Orders.Domain.Enums;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Feedbacks.Application.UseCases
{
    public class CreateFeedbackUseCase
    {
        private readonly IFeedbacksRepository _feedbackRepository;
        private readonly IOrderRepository _orderRepository;

        public CreateFeedbackUseCase(IFeedbacksRepository feedbackRepository, IOrderRepository orderRepository)
        {
            _feedbackRepository = feedbackRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Result<FeedbackResponseDTO>> CreateAsync(CreateFeedbackRequestDTO createFeedbackDTO)
        {
            var orderId = createFeedbackDTO.OrderId;
            var order = await _orderRepository.FindByIdAsync(orderId);

            if (order is null)
            {
                return Result<FeedbackResponseDTO>.Failure(
                    $"O pedido de Id '{orderId}' não foi encontrado",
                    ErrorType.NotFound
                );
            }

            var existingFeedback = await _feedbackRepository.FindByOrderIdAsync(orderId);

            if (existingFeedback is not null)
            {
                return Result<FeedbackResponseDTO>.Failure(
                    $"O pedido de Id '{orderId}' já possui um feedback",
                    ErrorType.CreateConflict
                );
            }

            if (order.OrderStatus != OrderStatus.Entregue)
            {
                return Result<FeedbackResponseDTO>.Failure(
                    $"O cliente só pode deixar um feedback se o pedido for entregue",
                    ErrorType.ValidationError
                );
            }

            var isSuccessFeedbackStarConversion = Enum.TryParse<FeedbackStar>(createFeedbackDTO.FeedbackStar, true, out var feedbackStar);

            if (!isSuccessFeedbackStarConversion)
            {
                return Result<FeedbackResponseDTO>.Failure(
                    $"Não foi possível fazer a conversão do tipo de feedback {createFeedbackDTO.FeedbackStar}",
                    ErrorType.ValidationError
                );
            }

            var feedback = Feedback.Create(
                orderId: orderId,
                comment: createFeedbackDTO.Comment,
                satisfaction: feedbackStar
            );

            var createResult = await _feedbackRepository.CreateAsync(feedback);

            if (!createResult.IsSuccess)
            {
                return Result<FeedbackResponseDTO>.Failure(
                    createResult.ErrorMessage ?? $"Não foi possível criar o feedback para o pedido de Id '{orderId}'",
                    createResult.ErrorType
                );
            }

            var feedbackResponseDTO = new FeedbackResponseDTO(
                Id: feedback.Id,
                OrderId: feedback.OrderId,
                Comment: feedback.Comment,
                Satisfaction: feedback.Satisfaction.ToString(),
                feedback.CreatedAt
            );

            return Result<FeedbackResponseDTO>.Success(feedbackResponseDTO);
        }
    }
}
