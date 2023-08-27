using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using Infrastructure.Persistence.Contexts;
using MassTransit;
using Workers.Consumers;
using Workers.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotificationRequestedConsumer>();
    x.AddConsumer<NotificationCreatedConsumer>();
    x.AddConsumer<NotificationSentConsumer>();
    x.AddConsumer<NotificationFailedConsumer>();
    x.AddDelayedMessageScheduler();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.UseDelayedMessageScheduler();
        cfg.ConfigureEndpoints(ctx);
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddHubs();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MeLinformaDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

app.Run();