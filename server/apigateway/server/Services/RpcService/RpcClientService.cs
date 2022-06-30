using System.Collections.Concurrent;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApiGateway.Services;

public class RpcClientService : IRpcClientService
{
    private IConnection? _connection;
    private IModel? _channel;
    private string? _replyQueueName;
    private EventingBasicConsumer? _consumer;
    private BlockingCollection<string> _respQueue = new BlockingCollection<string>();
    private IBasicProperties? _props;
    private readonly ILogger? _logger;
    private readonly string[] _topics = {"csharpmicroservice.test1","csharpmicroservice.test2"};

    public RpcClientService(IServiceScopeFactory serviceScopeFactory)
    {
        _logger = serviceScopeFactory
                    .CreateScope()
                    .ServiceProvider
                    .GetService<ILogger<RpcClientService>>();
    }

    public void InitialiseConnection() {
        var factory = new ConnectionFactory()  
        {   
            HostName = "messagebroker", 
            UserName = "admin",  
            Password = "admin",  
            Port = 5672
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: "micro_exchange", 
            type: "topic"
        );

        _replyQueueName = _channel.QueueDeclare(
            "rpc_reply_queue", 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null
        );

        _consumer = new EventingBasicConsumer(_channel);
        _props = _channel.CreateBasicProperties();
        
        var correlationId = Guid
                            .NewGuid()
                            .ToString();

        _props.CorrelationId = correlationId;
        _props.ReplyTo = _replyQueueName;

        if(_logger != null) {
            _logger.LogInformation("ApiGateway awaiting message...");
        }

        _consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);

            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                _respQueue.Add(response);
            }

            await Task.CompletedTask;
        };

        _channel.BasicConsume(
            consumer: _consumer,
            queue: _replyQueueName,
            autoAck: true
        );
    }

    public string SendRpcMessage(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: "micro_exchange",
            routingKey: _topics[0],
            basicProperties: _props,
            body: messageBytes
        );

        return _respQueue.Take();
    }

    public void SendMessage(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: "micro_exchange",
            routingKey: _topics[0],
            basicProperties: _props,
            body: messageBytes
        );
    }

    public void Close()
    {
        if(_connection != null) {
            _connection.Close();
        }
    }
}

