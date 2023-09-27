using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
using Pyco.Todo.Data.ViewModels;
using System.Collections.Concurrent;
using Pyco.Todo.DataAccess.Interfaces;
using Pyco.Todo.Data.Models;
using System.Security.Cryptography.Xml;

namespace Mqtt.Client.AspNetCore.Services;

public class MqttListItemUpdateService : IMqttClientService
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private readonly IConfiguration _configuration;
    private readonly IListItemRepository _itemRepository;
    private readonly ILogger<MqttListItemUpdateService> _logger;
    private readonly string _clientUpdateTopic;
    private readonly string _clientUpdateAckTopic;

    private Dictionary<int, List<ListItemClientUpdate>> _clientUpdates;
    private Dictionary<int, ListItem> _listItems;
    private Dictionary<int, int> _revisionIds;

    public MqttListItemUpdateService(
        MqttClientOptions options,
        IConfiguration configuration,
        IListItemRepository itemRepository,
        ILogger<MqttListItemUpdateService> logger)
    {
        _options = options;
        _configuration = configuration;
        _itemRepository = itemRepository;
        _mqttClient = new MqttFactory().CreateMqttClient();
        _logger = logger;
        _clientUpdateTopic = _configuration.GetValue<string>("Mqtt:ListClientUpdate");
        _clientUpdateAckTopic = _configuration.GetValue<string>("Mqtt:ListClientUpdateAck");

        _clientUpdates = new Dictionary<int, List<ListItemClientUpdate>>();
        _listItems = new Dictionary<int, ListItem>();

        ConfigureMqttClient();
    }

    private void ConfigureMqttClient()
    {
        _mqttClient.ConnectedAsync += HandleConnectedAsync;
        _mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
        _mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
    }

    public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        string content = eventArgs.ApplicationMessage.ConvertPayloadToString();
        ListItemClientUpdate? clientUpdate = JsonSerializer.Deserialize<ListItemClientUpdate>(content);

        if (clientUpdate is null)
        {
            return Task.CompletedTask;
        }

        // get the corresponding updates, listitem and revision from the stores
        if (!_clientUpdates.TryGetValue(clientUpdate.ListItemId, out List<ListItemClientUpdate>? listItemUpdates))
        {
            listItemUpdates = new List<ListItemClientUpdate>();
            _clientUpdates.Add(clientUpdate.ListItemId, listItemUpdates);
        }

        if (!_listItems.TryGetValue(clientUpdate.ListItemId, out ListItem? listItem))
        {
            listItem = _itemRepository.GetListItem(clientUpdate.ListItemId);

            if (listItem is null)
            {
                return Task.CompletedTask;
            }

            _listItems.Add(clientUpdate.ListItemId, listItem);
        }

        if (!_revisionIds.TryGetValue(clientUpdate.ListItemId, out int revision))
        {
            _revisionIds.Add(clientUpdate.ListItemId, revision);
        }

        // check client revision
        if (clientUpdate.LastSyncedRevision > revision)
        {
            return Task.CompletedTask;
        }
        else if (clientUpdate.LastSyncedRevision < revision)
        {
            TransformClientUpdate(clientUpdate, listItemUpdates);
        }

        // apply client changes
        ApplyClientUpdate(listItem, clientUpdate);
        clientUpdate.LastSyncedRevision = revision + 1;
        listItemUpdates.Add(clientUpdate);
        _revisionIds[clientUpdate.ListItemId] = revision + 1;

        // notify clients
        ListItemServerAck serverAck = new()
        {
            NewRevisionId = revision + 1,
            ListItemClientUpdate = clientUpdate,
        };
        PublishAcknowledge(serverAck);

        return Task.CompletedTask;
    }

    private void TransformClientUpdate(ListItemClientUpdate clientUpdate, List<ListItemClientUpdate> listItemUpdates)
    {
        var transformUpdates = listItemUpdates.Where(x => x.LastSyncedRevision > clientUpdate.LastSyncedRevision);

        foreach (var update in transformUpdates)
        {
            if (update.IsInsert && update.Position <= clientUpdate.Position)
            {
                clientUpdate.Position += update.Length;
            }
            else if (!update.IsInsert )
            {

            }
        }
    }

    private Task PublishAcknowledge(ListItemServerAck serverAck)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(_clientUpdateAckTopic)
            .WithPayload(JsonSerializer.Serialize(serverAck))
            .Build();

        return _mqttClient.PublishAsync(message);
    }

    private void ApplyClientUpdate(ListItem listItem, ListItemClientUpdate clientUpdate)
    {
        if (clientUpdate.IsInsert)
        {
            listItem.Content = listItem.Content.Insert(clientUpdate.Position, clientUpdate.Value);
        }
        else
        {
            int startIndex = clientUpdate.Position - clientUpdate.Length;
            listItem.Content = listItem.Content.Remove(startIndex, clientUpdate.Length);
        }
    }

    public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
    {
        _logger.LogInformation("connected");
        await _mqttClient.SubscribeAsync(_clientUpdateTopic);
    }

    public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
    {
        _logger.LogInformation("HandleDisconnected");
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        //await _mqttClient.ConnectAsync(_options);

        Task.Run(async () =>
        {
            // User proper cancellation and no while(true).
            while (true)
            {
                try
                {
                    // This code will also do the very first connect! So no call to _ConnectAsync_ is required in the first place.
                    if (!await _mqttClient.TryPingAsync())
                    {
                        await _mqttClient.ConnectAsync(_options, CancellationToken.None);

                        // Subscribe to topics when session is clean etc.
                        _logger.LogInformation("The MQTT client is connected.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception properly (logging etc.).
                    _logger.LogError(ex, "The MQTT client  connection failed");
                }
                finally
                {
                    // Check the connection state every 5 seconds and perform a reconnect if required.
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            var disconnectOption = new MqttClientDisconnectOptions
            {
                Reason = MqttClientDisconnectOptionsReason.NormalDisconnection,
                ReasonString = "NormalDiconnection"
            };
            await _mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
        }
        await _mqttClient.DisconnectAsync();
    }
}