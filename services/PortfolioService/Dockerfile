# -------- runtime --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# -------- build --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# 1️⃣ NuGet (если используешь GitHub Packages)
COPY nuget.config .

# 2️⃣ csproj
COPY PortfolioService.csproj .

# 3️⃣ restore
RUN dotnet restore PortfolioService.csproj

# 4️⃣ остальной код
COPY . .

RUN dotnet build PortfolioService.csproj -c $BUILD_CONFIGURATION -o /app/build

# -------- publish --------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish PortfolioService.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------- final --------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortfolioService.dll"]
