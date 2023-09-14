using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization.Attributes;
using Pyco.Todo.Core.Exception;
using Pyco.Todo.Core.Mqtt;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;
using System.Collections.Generic;

namespace Pyco.Todo.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/{controller}")]
public class ListItemController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IListItemDataProvider _listItemDataProvider;
    private readonly MqttHelper _mqttHelper;

    public ListItemController(
        IConfiguration configuration,
        IListItemDataProvider listItemDataProvider,
        MqttHelper mqttHelper)
    {
        _configuration = configuration;
        _listItemDataProvider = listItemDataProvider;
        _mqttHelper = mqttHelper;
    }

    [HttpPost]
    public async Task<IActionResult> Insert(ListItem listItem)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        int listItemId = _listItemDataProvider.Insert(listItem, user.Id);
        await _mqttHelper.Publish(_configuration.GetValue<string>("Mqtt:List") + listItem.ListId, string.Empty);
        return Ok(listItemId);
    }

    [HttpPut]
    public async Task<IActionResult> Update(ListItem listItem)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        _listItemDataProvider.Update(listItem, user.Id);
        await _mqttHelper.Publish(_configuration.GetValue<string>("Mqtt:List") + listItem.ListId, string.Empty);
        return Ok();
    }

    [HttpPut("delete")]
    public async Task<IActionResult> Delete(int listId, int listItemId)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        _listItemDataProvider.Archive(listItemId, listId, user.Id);
        await _mqttHelper.Publish(_configuration.GetValue<string>("Mqtt:List") + listId, string.Empty);
        return Ok();
    }

    [HttpPut("check")]
    public async Task<IActionResult> Check(int listId, int listItemId)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        _listItemDataProvider.Check(listId, listItemId, user.Id, true);
        await _mqttHelper.Publish(_configuration.GetValue<string>("Mqtt:List") + listId, string.Empty);
        return Ok();
    }

    [HttpPut("uncheck")]
    public async Task<IActionResult> Uncheck(int listId, int listItemId)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        _listItemDataProvider.Check(listId, listItemId, user.Id, false);
        await _mqttHelper.Publish(_configuration.GetValue<string>("Mqtt:List") + listId, string.Empty);
        return Ok();
    }
}
