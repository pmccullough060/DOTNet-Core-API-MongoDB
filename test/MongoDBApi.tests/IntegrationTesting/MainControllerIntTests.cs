using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MongoDBApi.IntegrationTesting
{
    public class MainControllerIntTests : IntegrationTest
    {
        [Fact]
        public async Task DatabaseInfo_ValidConnection()
        {
            //arrange
            await AuthenticateAsync();

            //act
            var url ="/Commands/DatabaseInfo";
            var response = await TestClient.GetAsync(url);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CollectionInfo_ValidConnection()
        {
            //arrange 
            await AuthenticateAsync();

            var url ="/commands/CollectionInfo?databaseName=test";
            var response = await TestClient.GetAsync(url);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}