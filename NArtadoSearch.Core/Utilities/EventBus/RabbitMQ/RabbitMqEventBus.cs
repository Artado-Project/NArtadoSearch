using System.Text;
using Microsoft.Extensions.Logging;
using NArtadoSearch.Core.Utilities.EventBus.Abstractions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NArtadoSearch.Core.Utilities.EventBus.RabbitMQ;

public class RabbitMqEventBus(IConnection connection, ILogger<RabbitMqEventBus> logger) : IEventBus
{
    private readonly IModel _model = connection.CreateModel();

    public void SendEvent<T>(T eventToSend) where T : class
    {
        var queueName = $"NArtadoSearch.{typeof(T).Name}";
        _model.QueueDeclare(
            queueName,
            true,
            false,
            false,
            new Dictionary<string, object>()
        );

        _model.ExchangeDeclare("NArtado.Exchange", ExchangeType.Direct, true, false);

        _model.QueueBind(queueName, "NArtado.Exchange", queueName, null);

        _model.BasicPublish("NArtado.Exchange", queueName, false, null,
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventToSend)));
    }

    public async Task SendEventAsync<T>(T eventToSend) where T : class
    {
        await Task.Run(() => SendEvent(eventToSend));
    }

    public async Task ConsumeAsync<T>(Action<T> onDataAvailable, CancellationToken cancellationToken) where T : class
    { 
        var queueName = $"NArtadoSearch.{typeof(T).Name}";
        _model.QueueDeclare(queueName, true, false, false);
        _model.ExchangeDeclare("NArtado.Exchange", ExchangeType.Direct, true, false);
        _model.QueueBind(queueName, "NArtado.Exchange", queueName, null);

        var consumer = new EventingBasicConsumer(_model);
        consumer.Received += (model, ea) =>
        {
            var bodyStr = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonConvert.DeserializeObject<T>(bodyStr);
            try
            {
                onDataAvailable?.Invoke(message);
                _model.BasicAck(ea.DeliveryTag, false);
            }
            catch(Exception ex)
            {
                logger.LogError("Error while processing message", ex);
            }
        };
        
        _model.BasicConsume(queueName, false, consumer);
    }
}