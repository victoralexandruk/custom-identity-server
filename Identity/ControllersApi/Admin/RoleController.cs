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
    public class RoleController : Controller
    {
        private readonly RoleRepository _roles;

        public RoleController(RoleRepository roles)
        {
            _roles = roles;
        }

        [HttpGet]
        public IEnumerable<Role> GetAll()
        {
            return _roles.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public Role Get(string id)
        {
            return _roles.FindById(id);
        }

        [HttpPut]
        public Role Save([FromBody] Role model)
        {
            _roles.Save(model);
            return model;
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            _roles.Delete(id);
            return Ok();
        }
    }
}
