#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["API/Evento.csproj", "API/"]
COPY ["ApplicationService/ApplicationService.csproj", "ApplicationService/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DataContract/DataContract.csproj", "DataContract/"]
COPY ["Persistance/DataPersistance.csproj", "Persistance/"]
RUN dotnet restore "API/Evento.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "Evento.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Evento.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Evento.Service.dll"]