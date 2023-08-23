FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Workers/Workers.csproj", "src/Workers/"]
RUN dotnet restore "src/Workers/Workers.csproj"
COPY . .
WORKDIR "/src/src/Workers"
RUN dotnet build "Workers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Workers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Workers.dll"]
