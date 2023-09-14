using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization.Attributes;
using Pyco.Todo.Core.Exception;
using Pyco.Todo.Core.Mqtt;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;

namespace Pyco.Todo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/v1/{controller}")]
    public class ListController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IListDataProvider _listDataProvider;
        private readonly IListRepository _listRepository;
        private readonly MqttHelper _mqttHelper;

        public ListController(
            IConfiguration configuration,
            IListDataProvider listDataProvider,
            IListRepository listRepository,
            MqttHelper mqttHelper)
        {
            _configuration = configuration;
            _listDataProvider = listDataProvider;
            _listRepository = listRepository;
            _mqttHelper = mqttHelper;
        }

        [HttpGet("user")]
        public IActionResult Get()
        {
            HttpContext.Items.TryGetValue("User", out object? obj);

            if (obj is null || obj is not User user)
            {
                throw new UnauthorizedException();
            }

            return Ok(_listRepository.GetByUser(user.Id));
        }

        [HttpGet]
        public IActionResult Get(int listId)
        {
            HttpContext.Items.TryGetValue("User", out object? obj);

            if (obj is null || obj is not User user)
            {
                throw new UnauthorizedException();
            }

            return Ok(_listDataProvider.Get(listId, user.Id));
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] List list)
        {
            int listId = _listDataProvider.Insert(list);

            foreach (var listUser in list.ListUsers ?? new List<User>())
            {
                await _mqttHelper.Publish(_configuration.GetValue<string>("Mqtt:User") + listUser.Id, string.Empty);
            }

            return Ok(listId);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] List list)
        {
            HttpContext.Items.TryGetValue("User", out object? obj);

            if (obj is null || obj is not User user)
            {
                throw new UnauthorizedException();
            }

            _listDataProvider.Update(list, user.Id);
            await _mqttHelper.Publish(_configuration.GetValue<string>("Mqtt:List") + list.Id, string.Empty);
            return Ok();
        }
    }
}
