using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Identity.ControllersApi
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = IdentityServerConstants.LocalApi.AuthenticationScheme)]
    public class TestController : ControllerBase
    {
        public IEnumerable<object> Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return claims;
        }
    }
}
