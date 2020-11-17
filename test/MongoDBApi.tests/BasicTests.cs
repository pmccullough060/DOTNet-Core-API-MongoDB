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
using MongoDBApi.Controllers;
using MongoDBApi.CRUD;
using Microsoft.AspNetCore.Mvc;

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

        [Fact]
        public void GetAllDatabases_Exists()
        {
            var mockCRUD = new Mock<IMongoCRUDOps>(); //mocking the database interaction layer

            var errorDetails = new ErrorDetails();

            mockCRUD.Setup(x => x.GetAllDatabases()).Returns("Success");

            var controller = new MainController(mockCRUD.Object);

            var actionResult = controller.DatabaseInfo();
            var contentResult = (OkObjectResult)actionResult;

            Assert.NotNull(contentResult);
            Assert.Equal(200, contentResult.StatusCode);
        }

        [Fact]
        public void GetAllDatabases_DontExist()
        {
            var mockCRUD = new Mock<IMongoCRUDOps>();

            
        }

    }
}
