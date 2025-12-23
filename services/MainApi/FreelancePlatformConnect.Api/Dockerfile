
# -------- base runtime --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# -------- build --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_TOKEN

WORKDIR /src

# 1️⃣ Копируем NuGet.Config
COPY nuget.config .
# 2️⃣ Обновляем credentials для github source
RUN dotnet nuget update source github \
    --username IvanovaArina \
    --password $GITHUB_TOKEN \
    --store-password-in-clear-text


# 2️⃣ Копируем csproj
COPY FreelancePlatformConnect.Api.csproj .

# 3️⃣ Restore с GitHub Packages
RUN dotnet restore FreelancePlatformConnect.Api.csproj

# 4️⃣ Копируем остальной код
COPY . .

RUN dotnet build FreelancePlatformConnect.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# -------- publish --------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish FreelancePlatformConnect.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------- final --------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FreelancePlatformConnect.Api.dll"]
