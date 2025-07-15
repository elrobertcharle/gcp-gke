docker.build.weather_api:
	docker build -t weather_api:1.0 -f build.WeatherApi.DockerFile .
	docker tag weather_api:1.0 us-central1-docker.pkg.dev/weather-4cee/weather-4cee-repo/weather_api:1.0
	docker compose -f ./Docker/api/docker-compose.yml up -d --force-recreate

docker.build.weather_api_migrations:
	docker build -t weather_api_migrations:1.0 -f build.WeatherApi_Migrations.DockerFile .
	docker tag weather_api_migrations:1.0 us-central1-docker.pkg.dev/weather-4cee/weather-4cee-repo/weather_api_migrations:1.0
