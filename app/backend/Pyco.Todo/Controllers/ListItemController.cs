using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization.Attributes;
using Pyco.Todo.Core.Exception;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;
using System.Collections.Generic;

namespace Pyco.Todo.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/{controller}")]
public class ListItemController : Controller
{
    private readonly IListItemDataProvider _listItemDataProvider;

    public ListItemController(IListItemDataProvider listItemDataProvider)
    {
        _listItemDataProvider = listItemDataProvider;
    }

    [HttpPost]
    public IActionResult Insert(ListItem listItem)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        return Ok(_listItemDataProvider.Insert(listItem, user.Id));
    }

    [HttpPut]
    public IActionResult Update(ListItem listItem)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        _listItemDataProvider.Update(listItem, user.Id);
        return Ok();
    }

    [HttpPut("delete")]
    public IActionResult Delete(int listId, int listItemId)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        _listItemDataProvider.Archive(listItemId, listId, user.Id);
        return Ok();
    }

    [HttpPut("check")]
    public IActionResult Check(int listId, int listItemId)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        _listItemDataProvider.Check(listId, listItemId, user.Id, true);
        return Ok();
    }

    [HttpPut("uncheck")]
    public IActionResult Uncheck(int listId, int listItemId)
    {
        HttpContext.Items.TryGetValue("User", out object? obj);

        if (obj is null || obj is not User user)
        {
            throw new UnauthorizedException();
        }

        _listItemDataProvider.Check(listId, listItemId, user.Id, false);
        return Ok();
    }
}
