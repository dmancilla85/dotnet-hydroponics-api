#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Hydroponics.Api/Hydroponics.Api.csproj", "Hydroponics.Api/"]
RUN dotnet restore "Hydroponics.Api/Hydroponics.Api.csproj"
COPY . .
WORKDIR "/src/Hydroponics.Api"
RUN dotnet build "Hydroponics.Api.csproj" -c Release -o /app/build

FROM build AS test
WORKDIR /src/Hydroponics.Tests
RUN dotnet test

FROM build AS publish
RUN dotnet publish "Hydroponics.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

ENV ASPNETCORE_ENVIRONMENT=Development

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hydroponics.Api.dll"]