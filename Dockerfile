FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiEntregasMentoria.csproj", "."]
RUN dotnet restore "ApiEntregasMentoria.csproj"
COPY . .
RUN dotnet build "ApiEntregasMentoria.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiEntregasMentoria.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Instalar PostgreSQL client para executar scripts
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

# Copiar script de inicialização
COPY init-db.sql .
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
