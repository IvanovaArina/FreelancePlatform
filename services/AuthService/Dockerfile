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
# Монтируем секрет и подставляем переменные в NuGet.Config временно
RUN --mount=type=secret,id=gh_packages_token \
    --mount=type=secret,id=github_actor \
    export GH_PACKAGES_TOKEN=$(cat /run/secrets/gh_packages_token) && \
    export GITHUB_ACTOR=$(cat /run/secrets/github_actor) && \
    sed -i "s/%GH_PACKAGES_TOKEN%/$GH_PACKAGES_TOKEN/g" nuget.config && \
    sed -i "s/%GITHUB_ACTOR%/$GITHUB_ACTOR/g" nuget.config && \
    dotnet restore services/AuthService/AuthService.csproj

# 2️⃣ Копируем csproj
COPY AuthService.csproj .

# 3️⃣ Restore с GitHub Packages
RUN dotnet restore AuthService.csproj

# 4️⃣ Копируем остальной код
COPY . .

RUN dotnet build AuthService.csproj -c $BUILD_CONFIGURATION -o /app/build

# -------- publish --------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish AuthService.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------- final --------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]
