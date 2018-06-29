using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Blue.IdentityServer
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                   ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" },
                    AllowOfflineAccess = true
                },

               // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    ClientName = "Blue App",
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 60,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" },
                    AllowOfflineAccess = true,
                    AllowedCorsOrigins = { "http://localhost:8100" }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "imp.client",
                    ClientName = "Blue App",
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 330,  // 330 seconds, default 60 minutes
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,

                    RedirectUris = new List<string> {
                        "http://localhost:5002/signin-oidc",
                        "http://localhost:8100/signin-oidc"
                    },

                    PostLogoutRedirectUris = new List<string> {
                        "http://localhost:5002/signout-callback-oidc",
                        "http://localhost:8100"
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:5200",
                        "http://localhost:8100"
                    }
                },

               // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "Blue App",
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 330,  // 330 seconds, default 60 minutes
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = new List<string> {
                        "http://localhost:5002/signin-oidc",
                        "http://localhost:8100/index.html",
                        "http://localhost:8100/signin-oidc"
                    },

                    PostLogoutRedirectUris = new List<string> {
                        "http://localhost:5002/signout-callback-oidc",
                        "http://localhost:8100"
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:5200",
                        "http://localhost:8100"
                    },

                    AllowOfflineAccess = true,
                    //AllowAccessTokensViaBrowser = false
                }
            };
        }
    }
}
