// DEV-NOTE: This file is our Node.js Kafka consumer. Its purpose is to connect to Kafka,
// listen to a specific topic, and process messages as they arrive, similar to our C# consumers.

const { Kafka } = require("kafkajs");

// DEV-NOTE: We read the Kafka broker address from environment variables for flexibility.
// If the variable isn't set, we default to the standard local Kafka address "localhost:9092".
const kafkaBrokers = process.env.KAFKA_BOOTSTRAP_SERVERS || "localhost:9092";

// 1. Create a Kafka client
// DEV-NOTE: This is the first step in using the kafkajs library. We create a new Kafka client instance,
// giving it a unique `clientId` for identification in logs and connecting it to our broker.
const kafka = new Kafka({
  clientId: "nodejs-consumer",
  brokers: [kafkaBrokers],
});

// 2. Create a consumer
// DEV-NOTE: We create a consumer instance here. The `groupId` is important. All consumers with the
// same `groupId` work together as a group to process messages from a topic. Kafka ensures that each
// message is delivered to only one consumer within the group.
const consumer = kafka.consumer({ groupId: "nodejs-test-group" });

// DEV-NOTE: We wrap our main logic in an async function `run` to use await for asynchronous operations.
const run = async () => {
  // Connect the consumer
  // DEV-NOTE: This establishes the network connection to our Kafka cluster.
  await consumer.connect();

  // Subscribe to the 'orders.new' topic
  // DEV-NOTE: Here, we tell the consumer which topic we are interested in.
  // `fromBeginning: true` means if this consumer group is new, it will read all historical messages in the topic.
  await consumer.subscribe({ topic: "orders.new", fromBeginning: true });
  console.log("Node.js consumer subscribed to topic 'orders.new'");

  // 3. Run the consumer to start fetching messages
  // DEV-NOTE: This is the heart of the consumer. The `.run()` method starts fetching messages from Kafka.
  // The `eachMessage` function is a callback that will be executed for every single message we receive.
  await consumer.run({
    eachMessage: async ({ topic, partition, message }) => {
      console.log("Message Received!");
      console.log({
        topic,
        partition,
        offset: message.offset,
        // DEV-NOTE: Messages from our CAP producer are sent as JSON strings.
        // We need to parse the message value from a buffer to a string, and then parse it as JSON to get the object.
        value: JSON.parse(message.value.toString()),
      });
      // DEV-NOTE: The "HELLO WORLD" part is purely for testing and can be customized or removed.
      const msg = `HELLO WORLD`;
      console.log(msg);
    },
  });
};

// DEV-NOTE: This block executes our `run` function and includes error handling. If anything goes wrong,
// it will log the error and exit the process, which is good practice for a background service.
run().catch((error) => {
  console.error("Error in Kafka consumer:", error);
  process.exit(1);
});

// Gracefully disconnect on shutdown
// DEV-NOTE: This is important for a clean shutdown. It listens for termination signals (like Ctrl+C)
// and ensures we disconnect the consumer from Kafka properly before the process exits.
process.on("SIGINT", async () => {
  console.log("Disconnecting consumer...");
  await consumer.disconnect();
  process.exit(0);
});