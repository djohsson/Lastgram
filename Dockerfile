#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-amd64 AS build

WORKDIR /src
COPY src/Lastgram/Lastgram.csproj ./Lastgram/Lastgram.csproj
COPY src/Core/Core.csproj ./Core/Core.csproj

WORKDIR /
COPY docker/LastgramDocker.sln .
RUN dotnet restore

COPY . .
WORKDIR /src/Lastgram
RUN dotnet build Lastgram.csproj -c Release -o /app/build

FROM build AS publish
# Set UseAppHost=false to avoid creating a rid specific executable.
# Without this, the executable would be linux-amd64 specific.
RUN dotnet publish Lastgram.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lastgram.dll"]
