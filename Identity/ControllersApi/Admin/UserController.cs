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
    public class UserController : Controller
    {
        private readonly UserRepository _users;

        public UserController(UserRepository users)
        {
            _users = users;
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _users.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public User Get(string id)
        {
            return _users.FindBySubjectId(id);
        }

        [HttpPut]
        public User Save([FromBody] User model)
        {
            _users.Save(model);
            return model;
        }

        [HttpPut]
        [Route("batch")]
        public IEnumerable<User> SaveBatch([FromBody] IEnumerable<User> models)
        {
            foreach (var model in models)
            {
                model.Id = _users.FindByUsername(model.UserName)?.Id;
                _users.Save(model);
            }
            return models;
        }
    }
}
