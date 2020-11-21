using Microsoft.AspNetCore.Mvc;
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
            var actionResult = controller.Login(mockUser);
            var contentResult = (OkObjectResult)actionResult;
            Assert.Equal(200, contentResult.StatusCode);
        }

        [Fact]
        public void Login_IncorrectDetails_Unauthorized()
        {
            var mockAuth = new Mock<IAuth>();
            var mockUser = new UserModel()
            {
                Username ="wrongmockName",
                Password ="wrongmockPassword"
            };
            var nullMockUser = new UserModel();
            nullMockUser = null;
            mockAuth.Setup(x => x.AuthenticateUser(mockUser)).Returns(nullMockUser); //return a null object for a dud login attempt
            var controller = new AuthenticationController(mockAuth.Object);
            var actionResult = controller.Login(mockUser);
            var contentResult = (UnauthorizedObjectResult)actionResult;
            Assert.Equal(401, contentResult.StatusCode);
        }
    }
}