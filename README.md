# Kafka Demo Project

This is a demo project we use to demonstrate a Kafka implementation with a .NET 9 producer and multiple consumers. The consumers are written in both C# and Node.js to showcase Kafka's ability to support a publish/subscribe pattern across different technology stacks.

The entire environment is orchestrated with Docker, so there's no need to install Kafka or any other dependencies on a local machine.

---
## Architecture

The demo is designed to showcase a **one-to-many (fan-out)** messaging pattern. The architecture is simple:

1. **A single Producer**: A .NET Web API (`kafka-api`) exposes an endpoint that, when called, publishes a message to a Kafka topic named `orders.new`.

2. **Multiple Consumers**: Four separate services are subscribed to the `orders.new` topic. When the producer sends a message, **all four consumers** receive their own copy of it.

---
## Services Included

Our `docker-compose.yml` file orchestrates the following services:

| Service | Container Name | Purpose | Access Point (from browser) |
| --- | --- | --- | --- |
| **kafka** | `(internal)` | The Kafka message broker | `(internal)` |
| **kafka-ui** | `kafka-ui` | **Universal Web Dashboard** | `http://localhost:8081` |
| **kafka-api** | `kafka-with-cap-example` | **.NET Producer API** & C# Consumer | `http://localhost:8080/swagger` |
| **kafka-consumer-svc** | `kafka-consumer-svc` | Standalone **C# Consumer #1** | `http://localhost:5176/cap` |
| **secondary-consumer** | `secondary-consumer-svc` | Standalone **C# Consumer #2** | `http://localhost:5083/cap` |
| **node-consumer** | `nodejs-kafka-consumer` | Standalone **Node.js Consumer** | `(logs only)` |

---
## How to Run the Demo

### Prerequisites
* Make sure **Docker** and **Docker Compose** are installed and running on the machine.

### Instructions
1. Open a terminal and navigate to the root directory of this project.
2. Run the appropriate build script for the operating system.

    * **On Linux, WSL, and macOS:**
        ```sh
        sudo sh build.sh
        ```

    * **On Windows (using PowerShell):**
        ```powershell
        .\build.ps1
        ```
---
## How to Use the Demo

### 1. Send a Message from the Producer
1. Open a web browser and navigate to the producer's Swagger UI:
   **[http://localhost:8080/swagger](http://localhost:8080/swagger)**
2. Expand the `POST /Order/create` endpoint.
3. Click **"Try it out"** and then **"Execute"**. This sends a message to the `orders.new` topic.

### 2. Monitor All Consumers with the Kafka UI
The best way to see everything working together is with the universal Kafka UI.

1. Open a new browser tab and navigate to:
   **[http://localhost:8081](http://localhost:8081)**
2. In the left-hand menu, click on **"Topics"** and then select `orders.new`.
3. Click on the **"Consumers"** tab for the topic.
4. We will see all the active consumer groups, including the `nodejs-test-group` for our Node.js service. We can see its lag and which partitions it's assigned to.
5. Click on the **"Messages"** tab to see the actual content of the messages that we've sent.

This dashboard gives us a complete, high-level view of our entire Kafka system in one place.

### 3. (Optional) View Individual Dashboards and Logs
* **CAP Dashboards**: For a view specific to each .NET service's perspective, we can still use their individual CAP dashboards:
    * Producer: `http://localhost:8080/cap`
    * Consumer #1: `http://localhost:5176/cap`
    * Consumer #2: `http://localhost:5083/cap`
* **Logs**: To see the raw console output from any service, we can use the `docker logs` command (e.g., `docker logs -f nodejs-kafka-consumer`).

---
## Stopping the Demo
To shut down all the running containers, press `Ctrl + C` in the terminal where the build script is running, or run `docker-compose down`.