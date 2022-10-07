#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/RateMeBotVk/RateMeBotVk.csproj", "src/RateMeBotVk/"]
COPY ["src/RateMeBotVk.DataAccess/RateMeBotVk.DataAccess.csproj", "src/RateMeBotVk.DataAccess/"]
RUN dotnet restore "src/RateMeBotVk/RateMeBotVk.csproj"
COPY . .
WORKDIR "/src/src/RateMeBotVk"
RUN dotnet build "RateMeBotVk.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RateMeBotVk.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RateMeBotVk.dll"]