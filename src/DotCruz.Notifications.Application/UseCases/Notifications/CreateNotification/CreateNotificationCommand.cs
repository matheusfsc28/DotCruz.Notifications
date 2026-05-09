using DotCruz.Notifications.Contracts.Messages.Notifications.CreateNotification;
using MediatR;

namespace DotCruz.Notifications.Application.UseCases.Notifications.CreateNotification;

public record CreateNotificationCommand(CreateNotificationMessage Message) : IRequest<Guid>;
