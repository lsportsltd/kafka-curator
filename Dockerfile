#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
COPY . .
# COPY ["src/Logic/KafkaCurator/KafkaCurator.csproj", "Logic/KafkaCurator/"]
# COPY ["src/Logic/KafkaCurator.Extensions/KafkaCurator.Extensions.csproj", "Logic/KafkaCurator.Extensions/"]
# COPY ["src/Logic/KafkaCurator.Core/KafkaCurator.Core.csproj", "Logic/KafkaCurator.Core/"]
RUN dotnet restore "src/Logic/LSports.Kafka.Curator/LSports.Kafka.Curator.csproj" --configfile "nuget/nuget.config"
# COPY . .
WORKDIR "/src/Logic/LSports.Kafka.Curator"
RUN dotnet build "LSports.Kafka.Curator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LSports.Kafka.Curator.csproj" -c Release -o /app/publish

FROM base AS final
ARG AWS_ACCESS_KEY_ID
ARG AWS_SECRET_ACCESS_KEY
ARG AWS_SESSION_TOKEN
ARG ENV
ENV ASPNETCORE_ENVIRONMENT=$ENV
ENV AWS_ACCESS_KEY_ID=$AWS_ACCESS_KEY_ID
ENV AWS_SECRET_ACCESS_KEY=$AWS_SECRET_ACCESS_KEY
ENV AWS_SESSION_TOKEN=$AWS_SESSION_TOKEN
WORKDIR /app
COPY --from=publish /app/publish .

RUN ["dotnet", "LSports.Kafka.Curator.dll"]
