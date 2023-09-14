using MQTTnet;
using MQTTnet.Client;

namespace Pyco.Todo.Core.Mqtt;

public class MqttHelper : IDisposable
{
    private readonly IMqttClient _mqttClient;

    public MqttHelper()
    {
        MqttFactory mqttFactory = new();
        _mqttClient = mqttFactory.CreateMqttClient();
    }

    public async Task Connect()
    {
        MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .WithCredentials("user1", "1234")
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
