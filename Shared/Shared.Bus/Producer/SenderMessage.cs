using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Shared.Bus.Producer;

public class SenderMessage<TContract>(string host, string queueName): ISenderMessage<TContract>
{
    private readonly ConnectionFactory _factory = new() { HostName = host };

    public Task Send(TContract msg)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(
            queueName,
            true,
            false,
            false,
            null);

        var body = JsonSerializer.Serialize(msg);
        channel.BasicPublish(
            string.Empty,
            queueName,
            null,
            Encoding.UTF8.GetBytes(body));

        return Task.CompletedTask;
    }
}