# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServerApp.csproj", "ServerApp/"]
RUN dotnet restore "ServerApp/ServerApp.csproj"
COPY --from=solution . .
WORKDIR "/src/ServerApp"
RUN dotnet build "./ServerApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# create dev-certificate
FROM build AS dev-certs
WORKDIR /app
RUN mkdir -p /.aspnet/https
RUN dotnet dev-certs https -v -ep /.aspnet/https/aspnetapp.pfx -p somepassword
RUN dotnet dev-certs https --trust -v
RUN chmod -R 444 /.aspnet

# test-build builds the xUnit integration test project
FROM build AS test-build
WORKDIR /src
COPY --from=tests --link . tests/
COPY /appsettings.json tests/
WORKDIR /src/tests
RUN dotnet restore
RUN dotnet build --no-restore

# test-entrypoint exposes tests as the default executable for the stage
FROM test-build AS integration-test
ENTRYPOINT ["dotnet", "test", "--no-build", "--logger:trx"]

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
WORKDIR "/src/ServerApp"
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ServerApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=dev-certs /.aspnet/https /app/.aspnet/https

ENTRYPOINT ["dotnet", "ServerApp.dll"]