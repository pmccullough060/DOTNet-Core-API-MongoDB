using MongoDBApi.AuthClasses;
using MongoDBApi.Controllers;
using MongoDBApi.Objects;
using Moq;
using Xunit;

namespace MongoDBApi.tests.ControllerUnitTests
{
    public class AuthenticationControllerUnitTests
    {
        [Fact]
        public void Login_CorrectDetails_OkObjectResult()
        {
            var mockAuth = new Mock<IAuth>();
            var mockUser = new UserModel()
            {
                Username ="mockName",
                Password ="mockPassword"
            };
            mockAuth.Setup(x => x.AuthenticateUser(mockUser)).Returns(mockUser);
            var controller = new AuthenticationController(mockAuth.Object);

        }
    }
}