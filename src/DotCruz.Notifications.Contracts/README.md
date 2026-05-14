# DotCruz.Notifications.Contracts

Pacote de contratos oficiais para integração com o serviço de Notificações da DotCruz. Este pacote contém as definições de mensagens e enums necessários para enviar notificações via RabbitMQ ou chamadas de API.

## Instalação

Você pode instalar o pacote via NuGet CLI:

```bash
dotnet add package DotCruz.Notifications.Contracts
```

Ou via Gerenciador de Pacotes do Visual Studio procurando por `DotCruz.Notifications.Contracts`.

## Conteúdo do Pacote

### Enums
- **IntegrationNotificationType**: Define o canal de envio (Email, Sms, Push).

### Mensagens (Mensagens de Integração)
- **CreateNotificationMessage**: Registro principal para solicitação de envio de notificações.

## Exemplo de Uso

Abaixo um exemplo de como instanciar a mensagem principal para envio via MassTransit ou outro broker:

```csharp
using DotCruz.Notifications.Contracts.Enums.Notifications;
using DotCruz.Notifications.Contracts.Messages.Notifications.CreateNotification;

var message = new CreateNotificationMessage(
    ServiceId: Guid.NewGuid(), // ID do seu serviço autenticado
    Type: IntegrationNotificationType.Email,
    Recipient: "usuario@exemplo.com",
    Title: "Bem-vindo!",
    Body: "Olá, seja bem-vindo ao sistema.",
    Culture: "pt-BR"
);

// Envie para a fila 'create-notification'
// await publishEndpoint.Publish(message);
```

### Uso com Templates

Se você preferir usar um template pré-configurado no serviço de notificações:

```csharp
var message = new CreateNotificationMessage(
    ServiceId: Guid.NewGuid(),
    Type: IntegrationNotificationType.Email,
    Recipient: "usuario@exemplo.com",
    TemplateCode: "WELCOME_EMAIL",
    TemplateData: new Dictionary<string, object> 
    {
        { "UserName", "Matheus" }
    }
);
```

## Configuração de Repositório

Este projeto faz parte do ecossistema DotCruz.
- **Repositório**: [GitHub DotCruz.Notifications](https://github.com/matheusfsc28/DotCruz.Notifications)
- **Autores**: Matheus Felipe @matheusfsc28
- **Empresa**: DotCruz
