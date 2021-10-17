#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Logic/KafkaCurator/KafkaCurator.csproj", "Logic/KafkaCurator/"]
COPY ["Logic/KafkaCurator.Extensions/KafkaCurator.Extensions.csproj", "Logic/KafkaCurator.Extensions/"]
COPY ["Logic/KafkaCurator.Core/KafkaCurator.Core.csproj", "Logic/KafkaCurator.Core/"]
RUN dotnet restore "Logic/KafkaCurator/KafkaCurator.csproj"
COPY . .
WORKDIR "/src/Logic/KafkaCurator"
RUN dotnet build "KafkaCurator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KafkaCurator.csproj" -c Release -o /app/publish

FROM base AS final
ARG env
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KafkaCurator.dll --env ${env}"]