using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Blue.Api
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
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("blue_api", "The Blue API scope")
            };

            //return new[]
            //{
            //    new ApiResource
            //    {
            //        Name = "blue_api_resource",
            //        DisplayName = "Blue Core API",
            //        ApiSecrets =
            //        {
            //            new Secret("secret".Sha256())
            //        },
            //        Scopes =
            //        {
            //            new Scope
            //            {
            //                Name = "blue_api.full_access",
            //                DisplayName = "Full access to Blue API"
            //            },
            //            new Scope
            //            {
            //                Name = "blue_api.read_only",
            //                DisplayName = "Read only access to Blue API"
            //            }
            //        }
            //    }
            //};
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            // client credentials client
            return new List<Client>
            {
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "eShop SPA OpenId Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =           { $"{clientsUrl["Spa"]}/" },
                    RequireConsent = false,
                    PostLogoutRedirectUris = { $"{clientsUrl["Spa"]}/" },
                    AllowedCorsOrigins =     { $"{clientsUrl["Spa"]}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "orders",
                        "basket",
                        "locations",
                        "marketing",
                        "webshoppingagg",
                        "orders.signalrhub"
                    }
                },

                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"api1"}
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    ClientName = "Blue Core App",
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 300,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"openid", "profile", "email", "blue_api" },
                    AllowOfflineAccess = true
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = true,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = {"http://localhost:5002/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5002/signout-callback-oidc"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}