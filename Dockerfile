# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY * /build/
WORKDIR /build/
RUN dotnet restore snipetrain-bot.csproj

# copy everything else and build app
RUN dotnet publish snipetrain-bot.csproj -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "snipetrain-bot.dll"]