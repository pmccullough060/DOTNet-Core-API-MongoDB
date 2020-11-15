using System;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Xunit;
using MongoDBApi.Objects;

namespace MongoDBApi.tests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTests( WebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task LoginTest() //testing the user logic returns a JWT token that allows access to an endpoint
        {
            string url = "/Authentication/Login";

            var user = new UserModel() //no need for dependency injection here...
            {
                Username = "Peter",
                Password = "Peter"
            };

            var client = _factory.CreateClient();

            var content = new StringContent(user.ToString(), Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(url, content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


    }
}
