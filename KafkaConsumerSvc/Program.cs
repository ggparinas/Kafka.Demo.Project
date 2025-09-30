// DEV-NOTE: This is the main entry point for our application. It's responsible for
// setting up all the necessary services, configuring the web server, and starting the app.

using KafkaConsumerSvc;

var builder = WebApplication.CreateBuilder(args);

// DEV-NOTE: These are standard services for building a web API.
// AddControllers registers our API controllers (like OrderController).
// AddEndpointsApiExplorer and AddSwaggerGen enable the Swagger UI for easy API testing.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DEV-NOTE: This line retrieves the Kafka server address from our configuration files (e.g., appsettings.json).
// If it's not specified, it defaults to "localhost:9092", the standard local Kafka port.
var kafkaServers = builder.Configuration["KafkaBootstrapServers"] ?? "localhost:9092";

// DEV-NOTE: This registers our MessageSubscriber class so the CAP library can find its message handling methods.
// 'AddScoped' means a new instance of MessageSubscriber is created for each web request.
builder.Services.AddScoped<MessageSubscriber>();

// DEV-NOTE: This is the core configuration for the CAP library, which simplifies how we use Kafka.
builder.Services.AddCap(options =>
{
    // DEV-NOTE: CAP needs a place to track message status. `UseInMemoryStorage` is perfect for demos
    // as it doesn't require us to set up a database. For production, we'd use a persistent store like a SQL database.
    options.UseInMemoryStorage();

    // DEV-NOTE: This tells CAP to use Kafka as its message broker and where to connect to it.
    options.UseKafka(kafkaServers);

    // DEV-NOTE: This enables the CAP Dashboard, a web interface accessible at '/cap'.
    // It's a fantastic tool for demos, allowing us to visually inspect messages, topics, and their status.
    options.UseDashboard();
});

// DEV-NOTE: This block configures the app's behavior in the development environment.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // DEV-NOTE: These lines enable the Swagger UI, which provides an interactive web page
    // for testing our API endpoints, like the one in OrderController.
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// DEV-NOTE: This maps incoming HTTP requests to our API controllers.
app.MapControllers();

app.Run();
