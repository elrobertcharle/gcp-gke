FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY ./WeatherDb/WeatherDb.csproj ./WeatherDb/WeatherDb.csproj
COPY ./WeatherApi/WeatherApi.csproj ./WeatherApi/WeatherApi.csproj

RUN dotnet restore "./WeatherDb/WeatherDb.csproj"
RUN dotnet restore "./WeatherApi/WeatherApi.csproj"

COPY . .

RUN dotnet publish "./WeatherApi/WeatherApi.csproj" -c Release -o /app/publish -p:InvariantGlobalization=true

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "WeatherApi.dll"]

#ENTRYPOINT ["tail", "-f", "/dev/null"]