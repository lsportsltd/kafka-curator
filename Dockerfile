#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /src
COPY . .
# COPY ["src/Logic/KafkaCurator/KafkaCurator.csproj", "Logic/KafkaCurator/"]
# COPY ["src/Logic/KafkaCurator.Extensions/KafkaCurator.Extensions.csproj", "Logic/KafkaCurator.Extensions/"]
# COPY ["src/Logic/KafkaCurator.Core/KafkaCurator.Core.csproj", "Logic/KafkaCurator.Core/"]
RUN dotnet restore "src/Logic/KafkaCurator/KafkaCurator.csproj"
# COPY . .
WORKDIR "/src/Logic/KafkaCurator"
RUN dotnet build "KafkaCurator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KafkaCurator.csproj" -c Release -o /app/publish

FROM base AS final
ARG AWS_ACCESS_KEY_ID
ARG AWS_SECRET_ACCESS_KEY
ARG AWS_SESSION_TOKEN
ARG ENV
ENV ENV=$ENV
ENV AWS_ACCESS_KEY_ID=$AWS_ACCESS_KEY_ID
ENV AWS_SECRET_ACCESS_KEY=$AWS_SECRET_ACCESS_KEY
ENV AWS_SESSION_TOKEN=$AWS_SESSION_TOKEN
WORKDIR /app
COPY --from=publish /app/publish .
RUN ["dotnet", "KafkaCurator.dll", "--env $ENV"]