docker.build.weather_api:
	docker build -t weather_api:1.0 -f build.WeatherApi.DockerFile .
	docker compose -f ./Docker/api/docker-compose.yml up -d --force-recreate

docker.build.weather_api_migrations:
	docker build -t weather_api_migrations:1.0 -f build.WeatherApi_Migrations.DockerFile .	