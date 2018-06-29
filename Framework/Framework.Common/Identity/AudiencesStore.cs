using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Framework.Common.Identity
{
    public class AudiencesStore
    {
        public static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();

        private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

        public AudiencesStore(string clientId, Audience audience)
        {
            AudiencesList.TryAdd(clientId, audience);
        }

        public Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");
            var key = new byte[32];

            _random.GetBytes(key);

            var base64Secret = Convert.ToBase64String(key);

            Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AudiencesList.TryAdd(clientId, newAudience);
            return newAudience;
        }

        public Audience FindAudience(string clientId)
        {
            if (AudiencesList.TryGetValue(clientId, out Audience audience))
            {
                return audience;
            }
            return null;
        }
    }
}
