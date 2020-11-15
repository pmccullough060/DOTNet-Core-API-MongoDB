using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace MongoDBApi.tests
{
    public class UnhandledExceptionTesting : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        
        public UnhandledExceptionTesting(WebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task UnhandledExceptionTest()
        {
            string url = "/commands/DatabaseInfo?connectionString=mongodb://localhost:27016"; //intentiaionally wrong
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url); 
            var responseContent = await response.Content.ReadAsStringAsync(); //converts the reponse message to a string
            Assert.Contains("Internal Server Error", responseContent); //Verifies our unhandled exception middleware is working
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}