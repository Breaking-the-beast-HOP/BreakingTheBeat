FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && \
    apt-get upgrade -y && \
    rm -rf /var/lib/apt/lists/*

USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["BreakingTheBeat/Gateway.csproj", "BreakingTheBeat/"]
COPY ["PodcastCatalog/PodcastCatalog.csproj", "BreakingTheBeat/PodcastCatalog/"]
COPY ["MusicCatalog/MusicCatalog.csproj", "BreakingTheBeat/MusicCatalog/"]
COPY ["SharedKernel/SharedKernel.csproj", "BreakingTheBeat/SharedKernel/"]
COPY ["UserManagement/UserManagement.csproj", "BreakingTheBeat/UserManagement/"]

RUN dotnet restore "./BreakingTheBeat/Gateway.csproj"

COPY . .

WORKDIR "/src/BreakingTheBeat"

RUN dotnet build "./Gateway.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./Gateway.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Gateway.dll"]