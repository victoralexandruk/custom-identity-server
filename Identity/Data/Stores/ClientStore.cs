using Identity.Data.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Threading.Tasks;

namespace Identity.Data.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly ClientRepository _clientRepository;

        public ClientStore(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _clientRepository.FindByClientId(clientId).ToIdentityClient();
            return Task.FromResult(client);
        }
    }
}
