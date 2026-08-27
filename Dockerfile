FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["GameStoreApi.csproj", "."]
RUN dotnet restore "GameStoreApi.csproj"

COPY . .
RUN dotnet publish "GameStoreApi.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet GameStoreApi.dll --urls http://0.0.0.0:${PORT:-8080}"]