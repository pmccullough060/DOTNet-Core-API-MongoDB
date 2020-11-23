using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDBApi.Objects;

namespace MongoDBApi.IntegrationTesting
{
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
            return registrationResponse;
        }

    }
}