# Stage 1: Build the migration bundle
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /src
COPY . .

RUN dotnet tool restore

# Run the bundle command inside the Alpine container. No --runtime flag is needed.
RUN dotnet ef migrations bundle --project WeatherDb --startup-project WeatherApi -o /app/efbundle --force

# Stage 2: Create the minimal final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app

# Copy ONLY the created bundle from the builder stage
COPY --from=build /app/efbundle .

# The entrypoint is just running the executable
ENTRYPOINT ["/app/efbundle"]