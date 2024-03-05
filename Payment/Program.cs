using System.Text;
using System.Text.Json;
using Payment.Web;
using RabbitMQ.Client.Events;
using Shared.Bus.Consumer;
using Shared.Contracts.Order;
using Shared.Contracts.Payment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var hostRabbit = builder.Configuration.GetValue<string>("host_rabbit");

if (string.IsNullOrWhiteSpace(hostRabbit)) throw new InvalidProgramException();
builder.Services.AddSingleton<ISenderOrder>((_) => new SenderOrder(hostRabbit, "paid_order"));

builder.Services.AddHostedService<ConsumerMessage>(service => new ConsumerMessage(hostRabbit, "order_to_pay",
    (sender, e) =>
    {
        var order =  JsonSerializer.Deserialize<MakePaymentContract>(e.Body.ToArray());
        if(order is null) return;
        var senderService = service.GetRequiredService<ISenderOrder>();
        senderService.Send(new PaidOrderContract()
        {
            OrderId = order.OrderId
        });
    }));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();