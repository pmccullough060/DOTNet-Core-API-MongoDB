using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDBApi.Objects;
using MongoDBApi.AuthClasses;
using Microsoft.Extensions.Logging;
using System;

namespace MongoDBApi.Controllers
{
    [Route("Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuth _auth;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuth auth, ILogger<AuthenticationController> logger)
        {
            _auth = auth ?? throw new ArgumentNullException(nameof(auth));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]UserModel login)
        {
            _logger.LogInformation("Login attempt made");

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