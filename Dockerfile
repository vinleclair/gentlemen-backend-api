# Build container
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.201-alpine as build-env

WORKDIR /aspnet-core
COPY ./aspnet-core.sln ./
COPY ./src/Gentlemen/Gentlemen.csproj ./src/Gentlemen/Gentlemen.csproj
COPY ./tests/Gentlemen.Tests/Gentlemen.Tests.csproj ./tests/Gentlemen.Tests/Gentlemen.Tests.csproj
RUN dotnet restore

# Copy everything else and build
COPY ./tests ./tests
COPY ./src ./src
RUN dotnet build -c Release --no-restore

RUN dotnet test "./tests/Gentlemen.Tests/Gentlemen.Tests.csproj" -c Release --no-build --no-restore
RUN dotnet publish "./src/Gentlemen/Gentlemen.csproj" -c Release -o out --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.3-alpine
WORKDIR /aspnet-core
COPY --from=build-env /aspnet-core/out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Gentlemen.dll