// DEV-NOTE: This is the main entry point for our consumer service. Its primary role is to configure
// and run the application, setting up the connection to Kafka so we can listen for messages.

using SecondaryConsumerSvc;

var builder = WebApplication.CreateBuilder(args);

// DEV-NOTE: These services are included but are not strictly necessary for a simple consumer.
// They would be used if we were building an API into this service.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DEV-NOTE: This line retrieves the Kafka server address from our configuration files (e.g., appsettings.json).
// It defaults to "localhost:9092" if no configuration is found.
var kafkaServers = builder.Configuration["KafkaBootstrapServers"] ?? "localhost:9092";

// DEV-NOTE: We register our MessageSubscriber class here. This allows the CAP library to discover
// the methods inside it that are set up to handle incoming messages.
builder.Services.AddScoped<MessageSubscriber>();

// DEV-NOTE: This is the core configuration for the CAP library, which manages the Kafka connection and message processing.
builder.Services.AddCap(options =>
{
    // DEV-NOTE: CAP uses this to track message state. `UseInMemoryStorage` is ideal for demos
    // because it requires no external database setup.
    options.UseInMemoryStorage();

    // DEV-NOTE: This crucial line tells CAP to use Kafka as its message broker and where to find it.
    options.UseKafka(kafkaServers);

    // DEV-NOTE: We're enabling the CAP Dashboard. It's a web UI at the '/cap' endpoint that lets us
    // visually monitor messages, which is very helpful for demonstrations.
    options.UseDashboard();
});

var app = builder.Build();

// DEV-NOTE: This section is for development-time features.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// DEV-NOTE: This maps controllers if we had any. In this consumer-only service, it doesn't have much effect.
app.MapControllers();

app.Run();
