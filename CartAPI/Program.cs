using CartAPI;
using CartAPI.Service;
using MassTransit;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<ICartService, CartService>();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AddToCartConsumer>(); // Register the consumer

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("add-to-cart-queue", e =>
        {
            e.ConfigureConsumer<AddToCartConsumer>(context); // Configure the consumer for the specified queue
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

//Eureka
app.UseDiscoveryClient();
app.UseHealthChecks("/info");

app.MapControllers();

app.Run();
