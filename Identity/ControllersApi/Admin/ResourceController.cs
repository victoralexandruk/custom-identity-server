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
    public class ResourceController : Controller
    {
        private readonly ResourceRepository _resources;

        public ResourceController(ResourceRepository resources)
        {
            _resources = resources;
        }

        [HttpGet]
        [Route("api")]
        public IEnumerable<CustomApiResource> GetAllApiResources()
        {
            return _resources.GetAllApiResources();
        }

        [HttpGet]
        [Route("api/{id}")]
        public CustomApiResource GetApiResource(string id)
        {
            return _resources.FindApiResourceById(id);
        }

        [HttpPut]
        [Route("api")]
        public CustomApiResource SaveApiResource([FromBody] CustomApiResource model)
        {
            _resources.SaveApiResource(model);
            return model;
        }

        [HttpDelete]
        [Route("api/{id}")]
        public IActionResult DeleteApiResource(string id)
        {
            _resources.DeleteApiResource(id);
            return Ok();
        }
    }
}
