﻿using Identity.Data.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Data.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly ResourceRepository _resourceRepository;

        public ResourceStore(ResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            if (apiResourceNames == null) throw new ArgumentNullException(nameof(apiResourceNames));
            return await _resourceRepository.FindApiResourcesByNameAsync(apiResourceNames);
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));
            return await _resourceRepository.FindApiResourcesByScopeNameAsync(scopeNames);
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));
            return await _resourceRepository.FindApiScopesByNameAsync(scopeNames);
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));
            return await _resourceRepository.FindIdentityResourcesByScopeNameAsync(scopeNames);
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            return await _resourceRepository.GetAllResourcesAsync();
        }
    }
}
