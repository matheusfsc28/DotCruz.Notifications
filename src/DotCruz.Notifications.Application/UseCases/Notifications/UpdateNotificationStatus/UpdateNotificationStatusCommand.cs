using DotCruz.Notifications.Contracts.Messages.Notifications.UpdateNotificationStatus;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Notifications.UpdateNotificationStatus;

public record UpdateNotificationStatusCommand(
    Guid NotificationId,
    UpdateNotificationStatusRequest Message
) : IRequest;
