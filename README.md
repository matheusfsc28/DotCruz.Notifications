# 🚀 DotCruz Notifications

![.NET 10](https://img.shields.io/badge/.NET-10.0-512bd4?style=for-the-badge&logo=dotnet)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-blue?style=for-the-badge)

  DotCruz Notifications é um microserviço robusto de alta performance projetado para centralizar a gestão e o disparo de notificações
  multiplataforma. Construído com as tecnologias mais recentes do ecossistema .NET, o sistema resolve o desafio de desacoplar o envio de
  mensagens (E-mail, SMS, Push) da lógica de negócio principal dos demais serviços da organização.

## 💡 Por que este projeto?

  Em arquiteturas distribuídas, delegar o envio de notificações para um serviço especializado garante:

- **Resiliência:** Se um provedor de e-mail falhar, o sistema utiliza políticas de re-tentativa (Retry) e filas para garantir a entrega.
- **Escalabilidade:** O processamento é feito em segundo plano (background), liberando a API para responder instantaneamente.
- **Padronização:** Gerenciamento centralizado de templates com suporte a variáveis dinâmicas e internacionalização (i18n).

## 🛠️ Stack Tecnológica & Padrões

- **Framework:** .NET 10 (C#)
- **Mensageria:** RabbitMQ + MassTransit
- **Persistência:** MongoDB (Driver 3.8.0)
- **Arquitetura:** Clean Architecture + DDD (Domain Driven Design)
- **Padrões de Projeto:**
  - Strategy Pattern: Para seleção dinâmica de provedores de envio (Email, SMS, Push).
  - Factory Strategy: Para criação de notificações baseada no tipo solicitado.
  - MediatR: Para implementação de CQRS e desacoplamento de eventos.
  - Polly: Políticas de retry incremental integradas ao MassTransit.

## 🏗️ Estrutura do Ecossistema

  O projeto é dividido em dois motores principais:

   1. **API (DotCruz.Notifications.Api):** Porta de entrada para cadastro de templates e comando inicial de disparo.
   2. **Worker (DotCruz.Notifications.Worker):** O cérebro do sistema. Contém:
       - Consumers: Processam as filas do RabbitMQ de forma assíncrona.
       - ScheduledNotificationPoller: Um serviço de background que monitora e dispara notificações agendadas para o futuro.

## 🚀 Como Executar (Zero Setup)

  A única dependência para rodar o projeto completo é o Docker.

  1. Iniciar o ecossistema
  Na raiz do projeto, execute:

```json
  docker-compose up -d --build
```

  1. Acessar as Interfaces
  Com o container rodando, você tem acesso imediato a:

- **📄 Documentação da API (Scalar):**
    <http://localhost:5050/scalar>
    Explore e teste os endpoints de criação de notificações e templates de forma interativa.

- **📧 Simulador de E-mail (Mailpit):**
    <http://localhost:8030/>
    Visualize em tempo real todos os e-mails disparados pelo sistema sem precisar de um servidor SMTP real.

- **🐰 RabbitMQ Dashboard:**
    <http://localhost:15675/> (User/Pass: guest)
    Acompanhe as filas e o fluxo de mensagens entre a API e o Worker.

## 🛠️ Variáveis de Ambiente

  O projeto já vem pré-configurado no docker-compose.yml, mas você pode customizar:

- **ConnectionStrings__MongoDb:** Conexão com o banco de dados.
- **Settings__RabbitMqSettings:** Configurações do broker.
- **Settings__ApiKey:** Chave de autenticação para os serviços.

## 🧪 Configuração para Testes Locais

  Para executar os testes automatizados que dependem da infraestrutura Docker (Testes de Integração/API), você deve criar o arquivo de
  configuração de ambiente na raiz do projeto DotCruz.Notifications.Api.

  1. Criar o arquivo appsettings.Test.json
  Local: src/DotCruz.Notifications.Api/appsettings.Test.json

```json
{
  "ConnectionStrings": {
    "MongoDb": "mongodb://localhost:27020"
  },
  "Settings": {
    "ApiKey": "DotCruzNotificationsKey",
    "EmailSettings": {
      "Host": "localhost",
      "Port": 1030,
        "Username": "",
        "Password": "",
        "FromEmail": "nao-responda@dotcruz.com",
        "FromName": "DotCruz Notifications",
        "EnableSsl": false
      },
      "MongoDbSettings": {
        "DatabaseName": "NotificationsDb"
      },
      "RabbitMqSettings": {
        "Host": "localhost",
        "Port": 5675,
        "Username": "guest",
        "Password": "guest"
      }
    }
  }
```

  > Nota: Utilizamos as portas 27020, 1030 e 5675 para que os testes rodando em sua máquina local consigam acessar os containers Docker
  expostos.

  1. Rodar os testes

```json
dotnet test
```

  ---
  Desenvolvido por Matheus Cruz – Focado em escalabilidade e sistemas distribuídos.
