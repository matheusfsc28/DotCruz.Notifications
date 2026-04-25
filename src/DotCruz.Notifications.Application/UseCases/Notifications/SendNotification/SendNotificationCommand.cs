using DotCruz.Notifications.Application.Common.Interfaces;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Notifications.SendNotification;

public record SendNotificationCommand(Guid NotificationId) : IRequest, INotificationCommand;
