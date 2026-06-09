# 🚀 DotCruz Notifications (API)

DotCruz Notifications é o serviço central de mensageria e gestão de notificações do ecossistema **DotCruz**. Construído em **.NET 10** utilizando **Clean Architecture** e princípios de **Domain-Driven Design (DDD)**, ele atua como a interface administrativa e API de entrada para criação de templates de notificação e agendamentos de disparos.

---

## ☁️ Arquitetura em Nuvem & Processamento

O sistema adota uma arquitetura distribuída e assíncrona baseada em serviços nativos de nuvem (AWS), desacoplando a recepção das requisições na API principal do envio físico das mensagens:

### 1. Fila de Processamento (Amazon SQS)
Todas as notificações enviadas imediatamente são publicadas como mensagens na fila **Amazon SQS**. Isso garante que o envio de mensagens seja assíncrono e resiliente, amortecendo picos de carga e permitindo retentativas em caso de falha.

### 2. Agendamento Efêmero (AWS EventBridge Scheduler)
Quando uma notificação é agendada para uma data futura, a API cria dinamicamente um agendamento de execução única no **AWS EventBridge Scheduler**. No instante configurado, o EventBridge encaminha a mensagem à fila SQS e se auto-destrói automaticamente (`ActionAfterCompletion.DELETE`), eliminando a necessidade de polling periódico em bancos de dados.

### 3. Armazenamento Seguro de Credenciais (AWS SSM Parameter Store)
As configurações de SMTP específicas de cada inquilino (*tenant*) são armazenadas com segurança no **SSM Parameter Store** como `SecureString`. Isso separa os segredos de conexão do banco de dados relacional principal e simplifica o gerenciamento multi-tenant.

### 4. Worker Desacoplado (AWS Lambda)
A leitura da fila SQS e a entrega real das notificações (E-mail, SMS, Push) não acontecem nesta API. Elas são de responsabilidade de um worker serverless otimizado compilado com Native AOT (consulte o repositório [DotCruz.Notifications.Delivery.Lambda](https://github.com/dotcruz-ecosystem/DotCruz.Notifications.Delivery.Lambda)).

---

## 🛠️ Stack Tecnológica & Padrões

*   **Framework**: .NET 10 (C#)
*   **Persistência**: MongoDB (Driver 3.x)
*   **Infraestrutura de Mensageria**: AWS SQS (Simple Queue Service)
*   **Agendamento de Eventos**: AWS EventBridge Scheduler
*   **Armazenamento de Segredos**: AWS Systems Manager (SSM) Parameter Store
*   **Padrões de Projeto**:
    *   **CQRS (MediatR)**: Separação clara de comandos e consultas.
    *   **Clean Architecture**: Desacoplamento rígido de dependências de infraestrutura por meio de interfaces.
    *   **Domain-Driven Design (DDD)**: Domínio rico com entidades, objetos de valor e conceitos de agregados.

---

## 🏗️ Estrutura do Projeto

O código do projeto está dividido nos seguintes subprojetos dentro da pasta `src`:

*   **[DotCruz.Notifications.Api](./src/DotCruz.Notifications.Api)**: Camada de apresentação que contém os endpoints Scalar, controllers e configuração inicial do ASP.NET.
*   **[DotCruz.Notifications.Application](./src/DotCruz.Notifications.Application)**: Regras de aplicação, Use Cases (envio, criação de templates, etc.) e interfaces de infraestrutura.
*   **[DotCruz.Notifications.Domain](./src/DotCruz.Notifications.Domain)**: Entidades de domínio (Templates, Notifications, Tenants), enums e exceções.
*   **[DotCruz.Notifications.Infrastructure](./src/DotCruz.Notifications.Infrastructure)**: Implementação de acesso a dados (MongoDB) e integrações de nuvem AWS (SQS, EventBridge Scheduler, SSM).
*   **[DotCruz.Notifications.CrossCutting](./src/DotCruz.Notifications.CrossCutting)**: Recursos de injeção de dependência globais e mapeamento de configurações.
*   **[DotCruz.Notifications.Contracts](./src/DotCruz.Notifications.Contracts)**: Modelos de mensagens e contratos compartilhados com outros microsserviços do ecossistema.

---

## 🚀 Como Executar em Desenvolvimento (Local Setup)

Para rodar a API localmente, você precisa subir a instância do MongoDB e configurar os acessos necessários aos recursos da AWS (SQS, EventBridge Scheduler e SSM).

### Pré-requisitos
*   Docker e Docker Compose instalados.
*   Credenciais de acesso à AWS com permissões para SQS, EventBridge Scheduler e SSM.

### Passo a Passo

1.  **Iniciar o Banco de Dados Local**:
    Na raiz do projeto, execute o comando para subir o MongoDB em segundo plano:
    ```bash
    docker compose up -d
    ```

2.  **Configurar Variáveis de Ambiente / Configurações Locais**:
    Ao rodar a API localmente (através do VS Code, Visual Studio ou via CLI `dotnet run`), configure as variáveis de ambiente necessárias para conexão com o MongoDB local e com a AWS (ou configure-as no seu gerenciador de User Secrets do .NET):
    ```json
    {
      "ConnectionStrings": {
        "MongoDb": "mongodb://localhost:27020"
      },
      "Settings": {
        "ApiKey": "DotCruzNotificationsKey",
        "AWS": {
          "Region": "us-east-1",
          "AccessKey": "SUA_ACCESS_KEY",
          "SecretKey": "SUA_SECRET_KEY",
          "SqsQueueArn": "arn:aws:sqs:us-east-1:123456789012:sua-fila",
          "SchedulerRoleArn": "arn:aws:iam::123456789012:role/seu-role-do-scheduler",
          "SmtpParameterPath": "/dotcruz/tenants/{0}/smtp-config"
        }
      }
    }
    ```

3.  **Acessar a Documentação da API (Scalar)**:
    Com a API executando localmente, você poderá acessar a documentação interativa em:
    *   **📄 Documentação da API (Scalar)**: `http://localhost:5050/scalar`

---

## 🔄 Deploy Contínuo (CI/CD)

O projeto possui duas esteiras automatizadas de implantação via GitHub Actions:

### 1. Implantação da API ([deploy.yml](./.github/workflows/deploy.yml))
Acionado a cada push para a branch `main`:
1.  Compila a imagem Docker a partir do [Dockerfile](./src/DotCruz.Notifications.Api/Dockerfile) e envia para o GitHub Container Registry (GHCR).
2.  Acessa a VPS de produção via SSH.
3.  Copia o [docker-compose.prod.yml](./docker-compose.prod.yml) para o servidor como `docker-compose.yml`.
4.  Injeta as variáveis de ambiente necessárias (como credenciais AWS, chaves de API e strings de conexão Mongo) a partir do GitHub Secrets.
5.  Executa `docker compose pull` e `docker compose up -d` para atualizar os contêineres sob a rede externa `dotcruz_net`.

### 2. Publicação de Contratos ([publish-contracts.yml](./.github/workflows/publish-contracts.yml))
Acionado quando há modificações na pasta de contratos:
1.  Gera o pacote NuGet (.nupkg) do projeto [DotCruz.Notifications.Contracts](./src/DotCruz.Notifications.Contracts/DotCruz.Notifications.Contracts.csproj).
2.  Publica o pacote no NuGet.org utilizando a chave de API fornecida em segredo.
