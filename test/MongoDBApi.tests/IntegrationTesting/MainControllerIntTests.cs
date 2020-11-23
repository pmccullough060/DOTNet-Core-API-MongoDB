using System.Threading.Tasks;
using Xunit;

namespace MongoDBApi.IntegrationTesting
{
    public class MainControllerIntTests : IntegrationTest
    {
        [Fact]
        public async Task DatabaseInfo()
        {
            //arrange
            await AuthenticateAsync();

            //act
            var url ="/Commands/DatabaseInfo";
            var response = await TestClient.GetAsync(url);

            //assert

        }
    }
}