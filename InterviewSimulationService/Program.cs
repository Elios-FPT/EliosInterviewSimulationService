using InterviewSimulation.Core.Interfaces;
using InterviewSimulation.Domain.Entities;
using InterviewSimulation.Infrastructure.DataContext;
using InterviewSimulation.Infrastructure.Implementations;
using InterviewSimulation.Infrastructure.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "InterviewSimulation API",
        Version = "v1",
        Description = "API utility operations"
    });
    c.AddServer(new OpenApiServer { Url = "/" });
});

// Add services to the container.
builder.Services.AddScoped<IAppConfiguration, AppConfiguration>();
builder.Services.AddScoped<ICombinedTransaction, CombinedTransaction>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IKafkaProducerRepository<>), typeof(KafkaProducerRepository<>));
builder.Services.AddScoped(typeof(IKafkaConsumerRepository<>), typeof(KafkaConsumerRepository<>));
builder.Services.AddScoped(typeof(IKafkaConsumerFactory<>), typeof(KafkaConsumerFactory<>));
builder.Services.AddScoped(typeof(IKafkaResponseHandler<>), typeof(KafkaResponseHandler<>));
builder.Services.AddScoped<IKafkaTransaction, KafkaTransaction>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<ISUtilityServiceClient, SUtilityServiceClient>(client =>
{
    string? serviceUrl = builder.Configuration["ServiceUrls:SUtilityService"];
    if (string.IsNullOrEmpty(serviceUrl))
    {
        throw new InvalidOperationException("SUtilityService URL is not configured in appsettings.json.");
    }
    client.BaseAddress = new Uri(serviceUrl);
});

// Database
builder.Services.AddDbContext<InterviewSimulationDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(InterviewSimulation.Core.AssemblyReference).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(InterviewSimulation.Contract.AssemblyReference).Assembly);
});

// Kafka Consumers
var sourceServices = builder.Configuration.GetSection("Kafka:SourceServices").Get<string[]>() ?? [];

Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss}] Registering {sourceServices.Length} Kafka consumers for sources: [{string.Join(", ", sourceServices)}]");

foreach (var sourceService in sourceServices)
{
    var currentSource = sourceService;

    builder.Services.AddSingleton<IHostedService>(sp =>
    {
        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

        return ActivatorUtilities.CreateInstance<KafkaConsumerHostedService<SessionQuestionAnswer>>(
            sp,
            scopeFactory,
            currentSource
        );
    });
}


var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    using var scope = app.Services.CreateScope();
    var appConfiguration = scope.ServiceProvider.GetRequiredService<IAppConfiguration>();
    KafkaResponseConsumer.Initialize(appConfiguration);
    Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss}] KafkaResponseConsumer initialized");
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "InterviewSimulation API v1");
    c.DocumentTitle = "InterviewSimulation API Documentation";
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();
app.MapControllers();

var currentService = builder.Configuration["KafkaCommunication:CurrentService"];
Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss}] {currentService} Service Started!");
Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss}] Kafka Consumers registered for: [{string.Join(", ", sourceServices)}]");

app.Run();