#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WhatsForDinner.Web/WhatsForDinner.Web.csproj", "src/WhatsForDinner.Web/"]
COPY ["lib/WhatsForDinner.SqlServer/WhatsForDinner.SqlServer.csproj", "lib/WhatsForDinner.SqlServer/"]
COPY ["lib/WhatsForDinner.Common/WhatsForDinner.Common.csproj", "lib/WhatsForDinner.Common/"]
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