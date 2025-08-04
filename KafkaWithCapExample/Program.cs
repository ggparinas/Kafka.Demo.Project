using KafkaWithCapExample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var kafkaServers = builder.Configuration["KafkaBootstrapServers"] ?? "localhost:9092";

builder.Services.AddScoped<MessageSubscriber>();

builder.Services.AddCap(options =>
{
    options.UseInMemoryStorage(); // We can also use DB storage, but I haven't explored that yet. -Gab
    options.UseKafka(kafkaServers); // This is important to be able to make use of kafka
    options.UseDashboard(); // Optional.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
