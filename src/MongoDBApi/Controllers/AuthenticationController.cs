using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDBApi.Objects;
using MongoDBApi.AuthClasses;

namespace MongoDBApi.Controllers
{
    [Route("Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuth _auth;

        public AuthenticationController(IAuth auth)
        {
            _auth = auth;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized(login);
            var user = _auth.AuthenticateUser(login);

            if(user != null)
            {
                var tokenString = _auth.GenerateJSONWebToken(user);
                response = Ok(new {token = tokenString});
            }
            return response;
        }
    }
}