FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiEntregasMentoria.csproj", "."]
RUN dotnet restore "ApiEntregasMentoria.csproj"
COPY . .
RUN dotnet publish "ApiEntregasMentoria.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ApiEntregasMentoria.dll"]
