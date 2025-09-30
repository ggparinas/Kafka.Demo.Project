using SecondaryConsumerSvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var kafkaServers = builder.Configuration["KafkaBootstrapServers"] ?? "localhost:9092";

builder.Services.AddScoped<MessageSubscriber>();

builder.Services.AddCap(options =>
{
    options.UseInMemoryStorage();
    options.UseKafka(kafkaServers);
    options.UseDashboard();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
