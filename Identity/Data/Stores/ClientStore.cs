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

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return await _clientRepository.FindClientByIdAsync(clientId);
        }
    }
}
