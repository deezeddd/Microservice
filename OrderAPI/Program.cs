using MassTransit;
using OrderAPI;
using OrderAPI.Service;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<IOrderService, OrderService>();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderConsumer>(); // Register the consumer

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("Order-queue", e =>
        {
            e.ConfigureConsumer<OrderConsumer>(context); // Configure the consumer for the specified queue
        });

    });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDiscoveryClient();
builder.Services.AddHealthChecks();

builder.Services.AddMassTransitHostedService();



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDiscoveryClient();
app.UseHealthChecks("/info");


app.MapControllers();

app.Run();
