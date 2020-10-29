using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace MongoDBApi.tests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTests( WebApplicationFactory<Startup> factory )
        {
            _factory = factory;
        }

        [Fact]
        public async Task ControllerClassTest()
        {
            string url = "/commands/DatabaseInfo?connectionString=mongodb://localhost:27017";
            var Client = _factory.CreateClient();
            var response = await Client.GetAsync(url);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}