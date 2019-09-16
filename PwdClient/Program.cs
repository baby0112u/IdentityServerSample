using System;
using System.Net.Http;
using IdentityModel.Client;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PwdClient {
    class Program {
        static async Task Main(string[] args) {

            // TokenClient 发现 
            var tokenClient = new HttpClient();
            var disco = await tokenClient.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError) {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await tokenClient.RequestPasswordTokenAsync(new PasswordTokenRequest() {
                Address = disco.TokenEndpoint,  // Address = disco.TokenEndpoint 请求Token的 TokenEndpoint
                ClientId = "pwdClient",         // ClientId 不能重复
                ClientSecret = "secret",
                UserName = "tanzb",
                Password = "password",
                Scope = "api"
            });

            if (tokenResponse.IsError) {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/api/values");
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            } else {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
