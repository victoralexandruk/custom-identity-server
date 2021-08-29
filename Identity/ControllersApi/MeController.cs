using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.ControllersApi
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class MeController : ControllerBase
    {
        public Dictionary<string, string> Get()
        {
            var claimsJson = new Dictionary<string, string>();
            foreach (Claim claim in User.Claims)
            {
                claimsJson[claim.Type] = claim.Value;
            }
            return claimsJson;
        }
    }
}
