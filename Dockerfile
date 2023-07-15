FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://*:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY *.sln .
COPY ["src/Construmart.Api/Construmart.Api.csproj", "src/Construmart.Api/"]
COPY ["src/Construmart.Core/Construmart.Core.csproj", "src/Construmart.Core/"]
COPY ["src/Construmart.Infrastructure/Construmart.Infrastructure.csproj", "src/Construmart.Infrastructure/"]
COPY ["test/Construmart.UnitTest/Construmart.UnitTest.csproj", "test/Construmart.UnitTest/"]
RUN dotnet restore --disable-parallel
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Construmart.Api.dll"]