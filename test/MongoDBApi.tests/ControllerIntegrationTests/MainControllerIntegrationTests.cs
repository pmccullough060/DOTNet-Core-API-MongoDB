using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using MongoDBApi.Objects;
using System.Net.Http;
using System.Text;

namespace MongoDBApi.tests.ControllerIntegrationTests
{
    public class MainControllerIntegrationtests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public MainControllerIntegrationtests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task AuthenticationController_LoginTest()
        {
            string url = "/Authentication/Login";

            var user = new UserModel()
            {
                Username = "Peter",
                Password = "Peter"
            };

            var client = _factory.CreateClient();

            var content = new StringContent(user.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
 











        }

    }
}