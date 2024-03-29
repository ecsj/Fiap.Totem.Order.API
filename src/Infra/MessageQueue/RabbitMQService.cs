using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infra.MessageQueue;

public class RabbitMQService : IMessageQueueService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQService(IConfiguration configuration)
    {
        
        var factory = new ConnectionFactory { Uri = new Uri(configuration.GetConnectionString("RabbitMQ")) };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void PublishMessage(string queue, string message)
    {
        _channel.QueueDeclare(queue, durable: true, false, false, null);

        var body = System.Text.Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: body
        );
    }

    public async Task ConsumeMessages(string queue, Func<string, Task> callback)
    {
        _channel.QueueDeclare(queue, durable: true, false, false, null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            await callback(message);
        };

        _channel.BasicConsume(queue, autoAck: true, consumer: consumer);
    }

    public void CloseConnection()
    {
        _channel.Close();
        _connection.Close();
    }
}