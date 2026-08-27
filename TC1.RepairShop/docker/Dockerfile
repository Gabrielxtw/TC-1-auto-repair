FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/TC1.RepairShop.Domain/TC1.RepairShop.Domain.csproj src/TC1.RepairShop.Domain/
COPY src/TC1.RepairShop.Application/TC1.RepairShop.Application.csproj src/TC1.RepairShop.Application/
COPY src/TC1.RepairShop.Infrastructure/TC1.RepairShop.Infrastructure.csproj src/TC1.RepairShop.Infrastructure/
COPY src/TC1.RepairShop.Api/TC1.RepairShop.Api.csproj src/TC1.RepairShop.Api/

RUN dotnet restore src/TC1.RepairShop.Api/TC1.RepairShop.Api.csproj

COPY src/ src/

RUN dotnet build src/TC1.RepairShop.Api/TC1.RepairShop.Api.csproj -c Release -o app/build

RUN dotnet publish src/TC1.RepairShop.Api/TC1.RepairShop.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

USER app
ENTRYPOINT ["dotnet", "TC1.RepairShop.Api.dll"]
