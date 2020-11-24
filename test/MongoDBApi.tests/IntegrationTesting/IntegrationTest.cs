using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using MongoDBApi.Objects;
using Newtonsoft.Json;

namespace MongoDBApi.IntegrationTesting
{
    //for Integration testing 


    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",await GetJwtAsync());
        } 

        protected async Task<string> GetJwtAsync()
        {
            string url = "/Authentication/Login";
            
            var user = new UserModel()
            {
                Username = "Peter",
                Password = "Peter"
            };

            var content = new StringContent(user.ToString(), Encoding.UTF8, "application/json");
            var response = await TestClient.PostAsync(url, content);
            var registrationResponse = await response.Content.ReadAsStringAsync();
            var jwtToken = JsonConvert.DeserializeObject<Token>(registrationResponse);
            return jwtToken.token;
        }
        private class Token //used to deserialise the token against, .NET loves to put escape characters in the strings which was throwing off Newtownsoft.
        {
            public string token;
        }
    }
}