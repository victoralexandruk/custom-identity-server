using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using IdentityModel;
using Identity.Custom.Constants;
using Identity.Data.Repositories;

namespace Identity.Custom
{
    public class CustomProfileService : IProfileService
    {
        protected readonly ILogger _logger;
        protected readonly UserRepository _userRepository;
        protected readonly RoleRepository _roleRepository;

        public CustomProfileService(ILogger<CustomProfileService> logger, UserRepository userRepository, RoleRepository roleRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }


        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            _logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);

            var user = _userRepository.FindBySubjectId(sub);

            var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.UserName, user.UserName),
                new Claim(JwtClaimTypes.Name, user.FullName),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Role, user.Role)
            };

            var client = context.Client;
            var role = _roleRepository.FindByName(user.Role);
            var permissionClaims = role?.Permissions?.Where(x => x.ClientId == client.ClientId).Select(permission => new Claim(CustomClaimTypes.Permission, permission.Name));
            if (permissionClaims != null)
                claims.AddRange(permissionClaims);

            //context.IssuedClaims = claims;
            context.IssuedClaims.AddRange(claims);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = _userRepository.FindBySubjectId(sub);
            context.IsActive = (user != null) && user.Active;

            return Task.CompletedTask;
        }
    }
}