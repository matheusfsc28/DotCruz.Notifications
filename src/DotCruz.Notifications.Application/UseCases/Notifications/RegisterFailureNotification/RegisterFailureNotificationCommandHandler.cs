using DotCruz.Notifications.Domain.Exceptions.BaseExceptions;
using DotCruz.Notifications.Domain.Exceptions.Resources;
using DotCruz.Notifications.Domain.Interfaces.Repositories;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Notifications.RegisterFailureNotification;

public class RegisterFailureNotificationCommandHandler : IRequestHandler<RegisterFailureNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;

    public RegisterFailureNotificationCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(RegisterFailureNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.NOTIFICATION_NOT_FOUND);

        notification.RegisterFailure(string.Join(" | ", request.ErrorsMessage));

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
    }
}
