using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity.Data
{
    public static class MemoryData
    {
        public static List<ApiScope> ApiScopes = new List<ApiScope>();

        public static List<ApiResource> Apis = new List<ApiResource>();
    }
}
