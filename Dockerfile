FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /working
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Eurocentric.WebApp/Eurocentric.WebApp.csproj", "src/Eurocentric.WebApp/"]
COPY ["src/Eurocentric.Apis.Admin/Eurocentric.Apis.Admin.csproj", "src/Eurocentric.Apis.Admin/"]
COPY ["src/Eurocentric.Apis.Public/Eurocentric.Apis.Public.csproj", "src/Eurocentric.Apis.Public/"]
COPY ["src/Eurocentric.Components/Eurocentric.Components.csproj", "src/Eurocentric.Components/"]
COPY ["src/Eurocentric.Domain/Eurocentric.Domain.csproj", "src/Eurocentric.Domain/"]
RUN dotnet restore "./src/Eurocentric.WebApp/Eurocentric.WebApp.csproj"
COPY . .
WORKDIR "/working/src/Eurocentric.WebApp"
RUN dotnet build "./Eurocentric.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Eurocentric.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eurocentric.WebApp.dll"]
