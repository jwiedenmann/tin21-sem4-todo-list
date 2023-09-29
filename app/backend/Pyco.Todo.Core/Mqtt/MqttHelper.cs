using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Pyco.Todo.Core.Mqtt;

public class MqttHelper : IDisposable
{
    private readonly IMqttClient _mqttClient;
    private readonly IConfiguration _configuration;

    public MqttHelper(IConfiguration configuration)
    {
        MqttFactory mqttFactory = new();
        _mqttClient = mqttFactory.CreateMqttClient();
        _configuration = configuration;
    }

    public async Task Connect()
    {
        MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
            .WithCredentials(
                _configuration.GetValue<string>("Mqtt:Config:Username"),
                _configuration.GetValue<string>("Mqtt:Config:Password"))
            .WithWebSocketServer(x => x.WithUri(_configuration.GetValue<string>("Mqtt:Config:Uri")))
            .Build();
        await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
    }

    public async Task Publish(string topic, string payload)
    {
        if (!_mqttClient.IsConnected)
        {
            await Connect();
        }

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
    }

    void IDisposable.Dispose()
    {
        _mqttClient.DisconnectAsync();
        _mqttClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
