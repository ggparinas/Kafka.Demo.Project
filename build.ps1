# This script stops, rebuilds, and restarts the Docker services.

Write-Host "Stopping and removing existing containers, networks, and local images..."
docker-compose down --rmi local --remove-orphans

Write-Host "Building service images..."
docker-compose build

Write-Host "Starting services in detached mode..."
docker-compose up -d

Write-Host "Following logs for specified services. Press Ctrl+C to exit."
docker-compose logs -f -t kafka kafka-api kafka-consumer-svc node-consumer