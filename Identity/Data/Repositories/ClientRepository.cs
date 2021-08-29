using Identity.Models;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data.Repositories
{
    public class ClientRepository
    {
        // Replce this with your Client persistence.
        private readonly List<CustomClient> _clients = MemoryData.Clients;

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _clients.FirstOrDefault(x => x.ClientId == clientId);
            return Task.FromResult(client?.ToIdentityClient());
        }
    }
}
