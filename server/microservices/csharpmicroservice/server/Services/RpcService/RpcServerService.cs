using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CSharpMicroservice.Services;

public class RpcServerService : IRpcServerService
{
    private IConnection? _connection;
    private IModel? _channel;
    private EventingBasicConsumer? _consumer;
    private readonly ILogger? _logger;
    private readonly string[] _topics = {"csharpmicroservice.test1","csharpmicroservice.test2"};

    public RpcServerService(IServiceScopeFactory serviceScopeFactory)
    {
        _logger = serviceScopeFactory
                    .CreateScope()
                    .ServiceProvider
                    .GetService<ILogger<RpcServerService>>();   
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

        _channel.QueueDeclare(
            queue: "rpc_queue", 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null
        );
        _channel.BasicQos(0, 1, false);

        foreach(string bindingKey in _topics)
        {
            _channel.QueueBind(queue: "rpc_queue",
                                exchange: "micro_exchange",
                                routingKey: bindingKey);
        }

        _consumer = new EventingBasicConsumer(_channel);

        _channel.BasicConsume(
            queue: "rpc_queue", 
            autoAck: false, 
            consumer: _consumer
        );

        if(_logger != null) {
            _logger.LogInformation("CSharpMicroservice awaiting message...");
        }

        _consumer.Received += async (model, ea) =>
        {
            string response = "";

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"CSharpMicroservice received message: {message}");

                response = message;
            }
            catch (Exception e)
            {
                response = "";

                if(_logger != null) {
                    _logger.LogInformation($"Exception: {e.Message}");
                }
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(response);

                _channel.BasicPublish(
                    exchange: "", 
                    routingKey: props.ReplyTo, 
                    basicProperties: replyProps, 
                    body: responseBytes
                );
                _channel.BasicAck(
                    deliveryTag: ea.DeliveryTag, 
                    multiple: false
                );
            }

            await Task.CompletedTask;
        };
    }

    public void Close()
    {
        if(_connection != null) {
            _connection.Close();
        }
    }
}
