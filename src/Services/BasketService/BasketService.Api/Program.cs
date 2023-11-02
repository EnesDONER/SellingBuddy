using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Infrastructure.Reporsitory;
using BasketService.Api.IntegrationEvents.EventHandlers;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.AddSingleton(sp=>sp.ConfigureRedis(builder.Configuration));
builder.Services.ConfigureConsul(builder.Configuration);

builder.Services.AddLogging(configure => { configure.AddConsole(); configure.AddDebug(); });
builder.Services.AddHttpContextAccessor();

//builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();
builder.Services.AddSingleton<IEventBus>(sp =>

    EventBusFactory.Create(new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "BasketService",
        EventBusType = EventBusType.RabbitMQ
    }, sp)

);
builder.Services.AddScoped<IBasketRepository, RedisBasketRepository>();
builder.Services.AddTransient<OrderCreatedIntegrationEventHandler>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

app.Start();
app.RegisterWithConsul(lifetime);
app.WaitForShutdown();
