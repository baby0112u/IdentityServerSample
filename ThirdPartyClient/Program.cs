using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ThirdPartyClient {
    class Program {
        static async Task Main(string[] args) {

            // TokenClient 发现 
            var tokenClient = new HttpClient();
            var disco = await tokenClient.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError) {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await tokenClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
                Address = disco.TokenEndpoint,  // Address = disco.TokenEndpoint 请求Token的 TokenEndpoint
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError) {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            } else {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
