#!/bin/sh
docker-compose down --rmi local --remove-orphans; 
docker-compose build;
docker-compose up -d;
docker-compose logs -f -t kafka kafka-api kafka-consumer-svc node-consumer