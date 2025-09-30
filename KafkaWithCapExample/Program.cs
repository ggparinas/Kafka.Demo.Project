// DEV-NOTE: This is the main entry point of our .NET application. 
// It sets up the web server and configures all the services that the application will use.
using KafkaWithCapExample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// DEV-NOTE: This pulls the Kafka server address from our configuration (e.g., appsettings.json).
// If it's not found, it defaults to "localhost:9092", the standard local Kafka port.
var kafkaServers = builder.Configuration["KafkaBootstrapServers"] ?? "localhost:9092";

// DEV-NOTE: This registers our MessageSubscriber class with the application's dependency injection system.
// This allows the CAP library to find and use it automatically to listen for messages.
builder.Services.AddScoped<MessageSubscriber>();

// DEV-NOTE: This section configures the CAP library, which is the heart of the Kafka integration.
builder.Services.AddCap(options =>
{
    // DEV-NOTE: CAP needs a way to store message state (e.g., sent, received, failed).
    // `UseInMemoryStorage` is great for demos because it requires no external database.
    // For a real application, we'd use a persistent storage like SQL Server or PostgreSQL.
    options.UseInMemoryStorage();

    // This is important to be able to make use of kafka
    // DEV-NOTE: This line tells CAP to use Kafka as the message broker and provides the server address.
    options.UseKafka(kafkaServers);

    // DEV-NOTE: This enables the CAP dashboard, a handy web UI to view message history, status, and topics.
    // It's very useful for debugging during a demo. We can access it at the '/cap' endpoint.
    options.UseDashboard();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // DEV-NOTE: These lines enable Swagger, which creates an interactive API documentation page.
    // It's perfect for demonstrating the `OrderController` endpoint without needing a separate tool like Postman.
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// DEV-NOTE: This tells the application to use the controllers we've defined, like `OrderController`.
app.MapControllers();

app.Run();
