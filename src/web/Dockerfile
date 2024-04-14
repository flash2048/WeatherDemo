#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY *.sln .
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Models/*.csproj ./Models/
COPY web/*.csproj ./web/

RUN dotnet restore "web/Weather.csproj"
COPY . .
RUN dotnet build "web/Weather.csproj" -c Release -o /app

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "web/Weather.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app ./
ENTRYPOINT ["dotnet", "Weather.dll"]