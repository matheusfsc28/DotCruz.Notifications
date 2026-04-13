using DotCruz.Notification.Domain.Entities.Base;
using DotCruz.Notification.Domain.Enums.Notifications;
using DotCruz.Notification.Exceptions;
using DotCruz.Notification.Exceptions.BaseExceptions;

namespace DotCruz.Notification.Domain.Entities.Templates;

public class Template : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Culture { get; private set; } = "pt-BR";
    public string DefaultSubject { get; private set; }
    public string Body { get; private set; } = string.Empty;
    public NotificationType Type { get; private set; }

    public Template(string code, string culture, string defaultSubject, string body, NotificationType type)
    {
        Code = code;
        Culture = !string.IsNullOrEmpty(culture) ? culture : Culture;
        DefaultSubject = defaultSubject;
        Body = body;
        Type = type;

        Validate();
    }

    public void Update(string? code, string? culture, string? defaultSubject, string? body, NotificationType? type)
    {
        Code = !string.IsNullOrEmpty(code) ? code : Code;
        Culture = !string.IsNullOrEmpty(culture) ? culture : Culture;
        DefaultSubject = !string.IsNullOrEmpty(defaultSubject) ? defaultSubject : DefaultSubject;
        Body = !string.IsNullOrEmpty(body) ? body : Body;
        Type = type ?? Type;

        Validate();
    }

    private void Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(Code))
            errors.Add(ResourceMessagesException.CODE_EMPTY);

        if (string.IsNullOrEmpty(Culture))
            errors.Add(ResourceMessagesException.CULTURE_EMPTY);

        if (string.IsNullOrEmpty(DefaultSubject))
            errors.Add(ResourceMessagesException.DEFAULT_SUBJECT_EMPTY);

        if (string.IsNullOrEmpty(Body))
            errors.Add(ResourceMessagesException.BODY_EMPTY);

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }
}
