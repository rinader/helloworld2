using System;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Crossover.Builder.Client.Providers
{
    public class OAuthClientProvider
    {
        private readonly Uri _uri;

        public OAuthClientProvider(string uri)
        {
            this._uri = new Uri(uri);
        }

        public async Task<TokenInfo> GetTokenInfo(string userName, string password)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.PostAsync(this._uri,
                    new StringContent(
                        string.Format("grant_type=password&username={0}&password={1}", userName, password),
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded"));
            }
            var message = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new SecurityException(string.Format("Security exception: {0}", message));
            }

            var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(message);
            return tokenInfo;
        }
    }

    public class TokenInfo
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty(".issued")]
        public string IssuedAt { get; set; }

        [JsonProperty(".expires")]
        public string ExpiresAt { get; set; }
    }
}