FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DotCruz.Notifications.Worker/DotCruz.Notifications.Worker.csproj", "src/DotCruz.Notifications.Worker/"]
COPY ["src/DotCruz.Notifications.Application/DotCruz.Notifications.Application.csproj", "src/DotCruz.Notifications.Application/"]
COPY ["src/DotCruz.Notifications.Domain/DotCruz.Notifications.Domain.csproj", "src/DotCruz.Notifications.Domain/"]
COPY ["src/DotCruz.Notifications.Infrastructure/DotCruz.Notifications.Infrastructure.csproj", "src/DotCruz.Notifications.Infrastructure/"]
COPY ["src/DotCruz.Notifications.CrossCutting/DotCruz.Notifications.CrossCutting.csproj", "src/DotCruz.Notifications.CrossCutting/"]
COPY ["src/DotCruz.Notifications.Exceptions/DotCruz.Notifications.Exceptions.csproj", "src/DotCruz.Notifications.Exceptions/"]
RUN dotnet restore "./src/DotCruz.Notifications.Worker/DotCruz.Notifications.Worker.csproj"
COPY . .
WORKDIR "/src/src/DotCruz.Notifications.Worker"
RUN dotnet build "./DotCruz.Notifications.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DotCruz.Notifications.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotCruz.Notifications.Worker.dll"]
