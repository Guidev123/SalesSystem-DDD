# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Web/SalesSystem.API/SalesSystem.API.csproj", "src/Web/SalesSystem.API/"]
COPY ["src/BuildingBlocks/SalesSystem.Email/SalesSystem.Email.csproj", "src/BuildingBlocks/SalesSystem.Email/"]
COPY ["src/BuildingBlocks/SalesSystem.EventSourcing/SalesSystem.EventSourcing.csproj", "src/BuildingBlocks/SalesSystem.EventSourcing/"]
COPY ["src/Services/SharedKernel/SalesSystem.SharedKernel/SalesSystem.SharedKernel.csproj", "src/Services/SharedKernel/SalesSystem.SharedKernel/"]
COPY ["src/Services/Catalog/SalesSystem.Catalog.Infrastructure/SalesSystem.Catalog.Infrastructure.csproj", "src/Services/Catalog/SalesSystem.Catalog.Infrastructure/"]
COPY ["src/Services/Catalog/SalesSystem.Catalog.Application/SalesSystem.Catalog.Application.csproj", "src/Services/Catalog/SalesSystem.Catalog.Application/"]
COPY ["src/Services/Catalog/SalesSystem.Catalog.Domain/SalesSystem.Catalog.Domain.csproj", "src/Services/Catalog/SalesSystem.Catalog.Domain/"]
COPY ["src/Services/Payments/SalesSystem.Payments.Infrastructure/SalesSystem.Payments.Infrastructure.csproj", "src/Services/Payments/SalesSystem.Payments.Infrastructure/"]
COPY ["src/Services/Payments/SalesSystem.Payments.ACL/SalesSystem.Payments.ACL.csproj", "src/Services/Payments/SalesSystem.Payments.ACL/"]
COPY ["src/Services/Payments/SalesSystem.Payments.Application/SalesSystem.Payments.Application.csproj", "src/Services/Payments/SalesSystem.Payments.Application/"]
COPY ["src/Services/Payments/SalesSystem.Payments.Domain/SalesSystem.Payments.Domain.csproj", "src/Services/Payments/SalesSystem.Payments.Domain/"]
COPY ["src/Services/Registers/SalesSystem.Registers.Infrastructure/SalesSystem.Registers.Infrastructure.csproj", "src/Services/Registers/SalesSystem.Registers.Infrastructure/"]
COPY ["src/Services/Registers/SalesSystem.Registers.Application/SalesSystem.Registers.Application.csproj", "src/Services/Registers/SalesSystem.Registers.Application/"]
COPY ["src/Services/Registers/SalesSystem.Registers.Domain/SalesSystem.Registers.Domain.csproj", "src/Services/Registers/SalesSystem.Registers.Domain/"]
COPY ["src/Services/Sales/SalesSystem.Sales.Infrastructure/SalesSystem.Sales.Infrastructure.csproj", "src/Services/Sales/SalesSystem.Sales.Infrastructure/"]
COPY ["src/Services/Sales/SalesSystem.Sales.Application/SalesSystem.Sales.Application.csproj", "src/Services/Sales/SalesSystem.Sales.Application/"]
COPY ["src/Services/Sales/SalesSystem.Sales.Domain/SalesSystem.Sales.Domain.csproj", "src/Services/Sales/SalesSystem.Sales.Domain/"]
RUN dotnet restore "./src/Web/SalesSystem.API/SalesSystem.API.csproj"
COPY . .
WORKDIR "/src/src/Web/SalesSystem.API"
RUN dotnet build "./SalesSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SalesSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesSystem.API.dll"]