FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["trash-tech-hub.sln", "."]
COPY ["trash-tech-hub.web/trash-tech-hub.web.csproj", "trash-tech-hub.web/"]
COPY ["trash-tech-hub.core/trash-tech-hub.core.csproj", "trash-tech-hub.core/"]
RUN dotnet restore "trash-tech-hub.web/trash-tech-hub.web.csproj"
COPY . .
WORKDIR "/src/trash-tech-hub.web"
RUN dotnet publish "trash-tech-hub.web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "trash-tech-hub.web.dll"]
