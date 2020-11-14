using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDBApi.Objects;
using MongoDBApi.AuthClasses;

namespace MongoDBApi.Controllers
{
    [Route("Authentication")]
    [ApiController]
    public class AuthenticationContoller : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuth _auth;

        public AuthenticationContoller(IConfiguration config, IAuth auth)
        {
            _config = config;
            _auth = auth;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
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