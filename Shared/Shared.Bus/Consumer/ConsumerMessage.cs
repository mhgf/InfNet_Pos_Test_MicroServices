using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Shared.Bus.Consumer;

public class ConsumerMessage : BackgroundService, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly EventHandler<BasicDeliverEventArgs> _handler;

    public ConsumerMessage(string host, string queueName, EventHandler<BasicDeliverEventArgs> handler)
    {
        _queueName = queueName;
        var factory = new ConnectionFactory() { HostName = host };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queueName,
            true,
            false,
            false,
            null);
        _handler = handler;
    }

    private void Register(EventHandler<BasicDeliverEventArgs> handler)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += handler;
        _channel.BasicConsume(queue: _queueName,
            autoAck: true,
            consumer: consumer);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += _handler;
        _channel.BasicConsume(queue: _queueName,
            autoAck: true,
            consumer: consumer);
        return Task.CompletedTask;;
    }

    public override void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
        _connection.Close();
        _channel.Close();
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        this.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}