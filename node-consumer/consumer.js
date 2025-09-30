const { Kafka } = require("kafkajs");

// Read the Kafka broker address from environment variables
const kafkaBrokers = process.env.KAFKA_BOOTSTRAP_SERVERS || "localhost:9092";

// 1. Create a Kafka client
const kafka = new Kafka({
  clientId: "nodejs-consumer",
  brokers: [kafkaBrokers],
});

// 2. Create a consumer
const consumer = kafka.consumer({ groupId: "nodejs-test-group" });

const run = async () => {
  // Connect the consumer
  await consumer.connect();

  // Subscribe to the 'orders.new' topic
  await consumer.subscribe({ topic: "orders.new", fromBeginning: true });
  console.log("Node.js consumer subscribed to topic 'orders.new'");

  // 3. Run the consumer to start fetching messages
  await consumer.run({
    eachMessage: async ({ topic, partition, message }) => {
      console.log("Message Received!");
      console.log({
        topic,
        partition,
        offset: message.offset,
        // The message value from CAP is a JSON string, so we parse it
        value: JSON.parse(message.value.toString()),
      });
      const msg = `HELLO WORLD`;
      console.log(msg);
    },
  });
};

run().catch((error) => {
  console.error("Error in Kafka consumer:", error);
  process.exit(1);
});

// Gracefully disconnect on shutdown
process.on("SIGINT", async () => {
  console.log("Disconnecting consumer...");
  await consumer.disconnect();
  process.exit(0);
});