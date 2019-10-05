FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ElGuerre.Microservices.Billing.Api/ElGuerre.Microservices.Billing.Api.csproj", "ElGuerre.Microservices.Billing.Api/"]
RUN dotnet restore "ElGuerre.Microservices.Billing.Api/ElGuerre.Microservices.Billing.Api.csproj"
COPY . .
WORKDIR "/src/ElGuerre.Microservices.Billing.Api"
RUN dotnet build "ElGuerre.Microservices.Billing.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ElGuerre.Microservices.Billing.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ElGuerre.Microservices.Billing.Api.dll"]