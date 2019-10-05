FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ElGuerre.Microservices.Shipping.Api/ElGuerre.Microservices.Shipping.Api.csproj", "ElGuerre.Microservices.Shipping.Api/"]
RUN dotnet restore "ElGuerre.Microservices.Shipping.Api/ElGuerre.Microservices.Shipping.Api.csproj"
COPY . .
WORKDIR "/src/ElGuerre.Microservices.Shipping.Api"
RUN dotnet build "ElGuerre.Microservices.Shipping.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ElGuerre.Microservices.Shipping.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ElGuerre.Microservices.Shipping.Api.dll"]