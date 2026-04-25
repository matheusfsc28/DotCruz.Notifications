using DotCruz.Notifications.Application.Common.Interfaces;
using DotCruz.Notifications.CrossCutting.Resources;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using DotCruz.Notifications.Exceptions.BaseExceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DotCruz.Notifications.Application.Common.Behaviors;

public class NotificationFailureBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : INotificationCommand
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationFailureBehavior<TRequest, TResponse>> _logger;

    public NotificationFailureBehavior(
        INotificationRepository notificationRepository,
        ILogger<NotificationFailureBehavior<TRequest, TResponse>> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (NotificationException)
        {
            // Exceções de domínio/validação não devem registrar falha de envio técnica aqui, 
            // pois geralmente são tratadas por um ExceptionFilter global na API.
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ResourceLogMessages.ERROR_SENDING_NOTIFICATION, request.NotificationId);

            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken);
            
            if (notification != null)
            {
                notification.RegisterFailure(ex.Message);
                await _notificationRepository.UpdateAsync(notification, cancellationToken);
            }

            throw; // Re-throw para o MassTransit ou Caller saber que falhou
        }
    }
}
