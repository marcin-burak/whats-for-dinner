FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WhatsForDinner.Api/WhatsForDinner.Api.csproj", "src/WhatsForDinner.Api/"]
COPY ["lib/WhatsForDinner.SqlServer/WhatsForDinner.SqlServer.csproj", "lib/WhatsForDinner.SqlServer/"]
COPY ["lib/WhatsForDinner.Common/WhatsForDinner.Common.csproj", "lib/WhatsForDinner.Common/"]
RUN dotnet restore "./src/WhatsForDinner.Api/WhatsForDinner.Api.csproj"
COPY . .
WORKDIR "/src/src/WhatsForDinner.Api"
RUN dotnet build "./WhatsForDinner.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WhatsForDinner.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WhatsForDinner.Api.dll"]