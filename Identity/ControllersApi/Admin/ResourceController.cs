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
        public IEnumerable<CustomApiResource> GetAll()
        {
            return _resources.GetAllApiResources();
        }

        [HttpGet]
        [Route("api/{id}")]
        public CustomApiResource Get(string id)
        {
            return _resources.FindApiResourceById(id);
        }
    }
}
