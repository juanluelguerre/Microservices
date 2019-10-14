FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ElGuerre.Microservices.Sales.Api/ElGuerre.Microservices.Sales.Api.csproj", "ElGuerre.Microservices.Sales.Api/"]
RUN dotnet restore "ElGuerre.Microservices.Sales.Api/ElGuerre.Microservices.Sales.Api.csproj"
COPY . .
WORKDIR "/src/ElGuerre.Microservices.Sales.Api"
RUN dotnet build "ElGuerre.Microservices.Sales.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ElGuerre.Microservices.Sales.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ElGuerre.Microservices.Sales.Api.dll"]