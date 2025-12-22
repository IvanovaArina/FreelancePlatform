# -------- base runtime --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# ImageService не имеет HTTP API → порт не обязателен
# (можно оставить для логов / будущего)
EXPOSE 8080

# -------- build --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# 1️⃣ NuGet.Config (GitHub Packages)
COPY nuget.config .

# 2️⃣ csproj
COPY ImageService.csproj .

# 3️⃣ restore
RUN dotnet restore ImageService.csproj

# 4️⃣ код
COPY . .

# 5️⃣ build
RUN dotnet build ImageService.csproj -c $BUILD_CONFIGURATION -o /app/build

# -------- publish --------
FROM build AS publish
RUN dotnet publish ImageService.csproj -c Release -o /app/publish /p:UseAppHost=false

# -------- final --------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ImageService.dll"]
