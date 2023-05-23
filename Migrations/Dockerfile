FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["migrations/IdentityServerConfig.Migrations.csproj", "migrations/"]
RUN dotnet restore "migrations/IdentityServerConfig.Migrations.csproj"
COPY . .
WORKDIR "/src/migrations"
RUN dotnet build "IdentityServerConfig.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServerConfig.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServerConfig.Migrations.dll"]
