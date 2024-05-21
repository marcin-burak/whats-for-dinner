FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WhatsForDinner.Web/WhatsForDinner.Web.csproj", "src/WhatsForDinner.Web/"]
RUN dotnet restore "./src/WhatsForDinner.Web/WhatsForDinner.Web.csproj"
COPY . .
WORKDIR "/src/src/WhatsForDinner.Web"
RUN dotnet build "./WhatsForDinner.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WhatsForDinner.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WhatsForDinner.Web.dll"]