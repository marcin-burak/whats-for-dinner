FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WhatsForDinner.Blazor/WhatsForDinner.Blazor/WhatsForDinner.Blazor.csproj", "src/WhatsForDinner.Blazor/WhatsForDinner.Blazor/"]
COPY ["src/WhatsForDinner.Blazor/WhatsForDinner.Blazor.Client/WhatsForDinner.Blazor.Client.csproj", "src/WhatsForDinner.Blazor/WhatsForDinner.Blazor.Client/"]
RUN dotnet restore "./src/WhatsForDinner.Blazor/WhatsForDinner.Blazor/WhatsForDinner.Blazor.csproj"
COPY . .
WORKDIR "/src/src/WhatsForDinner.Blazor/WhatsForDinner.Blazor"
RUN dotnet build "./WhatsForDinner.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WhatsForDinner.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WhatsForDinner.Blazor.dll"]