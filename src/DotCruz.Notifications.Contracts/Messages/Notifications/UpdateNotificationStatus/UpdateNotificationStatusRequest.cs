namespace DotCruz.Notifications.Contracts.Messages.Notifications.UpdateNotificationStatus;

public record UpdateNotificationStatusRequest(
    bool Success, 
    string? ErrorMessage
);
