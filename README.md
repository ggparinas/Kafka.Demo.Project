# Kafka.Demo.Project

This is the demo project used to demonstrate the .NET 9 implementation of Kafka. This project also includes a node.js project that was used to showcase Kafka's ability to support multiple consumers being subscribed to a single producer.

You may run this without having to install kafka in your local machine by running it via Docker.

## How to run via Docker

### On Linux, WSL, and macOS:

1. Make sure you're in the root directory of this project (i.e. outside of KafkaWithCapExample and node-consumer, and in the same level as the docker-compose.yml file)
2. run the following command:
```sudo sh build.sh```

### On Windows:

1. Make sure you're in the root directory of this project (i.e. outside of KafkaWithCapExample and node-consumer, and in the same level as the docker-compose.yml file)
2. run the following command:
```.\build.ps1```
