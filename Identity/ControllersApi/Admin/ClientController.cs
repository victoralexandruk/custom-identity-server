using Identity.Data.Repositories;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Identity.ControllersApi.Admin
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ClientRepository _clients;

        public ClientController(ClientRepository clients)
        {
            _clients = clients;
        }

        [HttpGet]
        public IEnumerable<CustomClient> GetAll()
        {
            return _clients.GetAll();
        }

        [HttpGet]
        [Route("{clientId}")]
        public CustomClient Get(string clientId)
        {
            return _clients.FindByClientId(clientId);
        }

        [HttpPut]
        public CustomClient Save([FromBody] CustomClient model)
        {
            _clients.Save(model);
            return model;
        }

        [HttpDelete]
        [Route("{clientId}")]
        public IActionResult Delete(string clientId)
        {
            _clients.Delete(clientId);
            return Ok();
        }
    }
}
