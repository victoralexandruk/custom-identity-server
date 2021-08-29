using Identity.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data
{
    public class ClientStore : IClientStore
    {
        // Replce this with your user persistence. 
        private readonly IEnumerable<CustomClient> _clients = new List<CustomClient>
        {
            new CustomClient
            {
                ClientId = "client",
                ClientName = "Demo Client",
                LogoUri = "",
                ClientSecret = new Secret("secret".Sha256()),
                AllowedUris = { "https://localhost:5005" },
                FrontChannelLogoutUri = "https://localhost:5005/Home/Signout"
            }
        };

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _clients.FirstOrDefault(x => x.ClientId == clientId);
            return Task.FromResult(client?.ToIdentityClient());
        }
    }
}
