using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization.Attributes;
using Pyco.Todo.Core.Exception;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;
using System.Collections.Generic;

namespace Pyco.Todo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/v1/{controller}")]
    public class ListController : Controller
    {
        private readonly IListDataProvider _listDataProvider;
        private readonly IListRepository _listRepository;

        public ListController(
            IListDataProvider listDataProvider,
            IListRepository listRepository)
        {
            _listDataProvider = listDataProvider;
            _listRepository = listRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            HttpContext.Items.TryGetValue("User", out object? obj);

            if (obj is null || obj is not User user)
            {
                throw new UnauthorizedException();
            }

            return Ok(_listRepository.Get(user.Id));
        }

        [HttpGet]
        public IActionResult Get(int listId)
        {
            HttpContext.Items.TryGetValue("User", out object? obj);

            if(obj is null || obj is not User user)
            {
                throw new UnauthorizedException();
            }

            return Ok(_listDataProvider.Get(listId, user.Id));
        }

        [HttpPost]
        public IActionResult Insert([FromBody] List list) => Ok(_listDataProvider.Insert(list));

        [HttpPut]
        public IActionResult Update([FromBody] List list)
        {
            HttpContext.Items.TryGetValue("User", out object? obj);

            if (obj is null || obj is not User user)
            {
                throw new UnauthorizedException();
            }

            _listDataProvider.Update(list, user.Id);

            return Ok();
        }
    }
}
