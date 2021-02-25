#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Lastgram/Lastgram.csproj", "Lastgram/"]
RUN dotnet restore "Lastgram/Lastgram.csproj"
COPY . .
WORKDIR "/src/Lastgram"
RUN dotnet build "Lastgram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lastgram.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lastgram.dll"]