# Use the official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VirtualAssistant.API.csproj", "./"]
RUN dotnet restore "VirtualAssistant.API.csproj"
COPY . .
RUN dotnet build "VirtualAssistant.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VirtualAssistant.API.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VirtualAssistant.API.dll"]