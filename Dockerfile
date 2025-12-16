FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Install PostgreSQL client for database operations
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiEntregasMentoria.csproj", "."]
RUN dotnet restore "ApiEntregasMentoria.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ApiEntregasMentoria.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiEntregasMentoria.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install PostgreSQL client for database operations
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Copy database initialization files
COPY init-db.sql /app/init-db.sql
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]
