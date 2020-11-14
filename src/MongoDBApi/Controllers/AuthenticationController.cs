using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MongoDBApi.Objects;

namespace MongoDBApi.Controllers
{
    [Route("Authentication")]
    [ApiController]
    public class AuthenticationContoller : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthenticationContoller(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if(user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new {token = tokenString});
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] //adding data to the new token, that will be returned to the user.
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                new Claim("DateOfJoining", userInfo.DateOfJoining.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["JWT:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            if(login.Username == "Peter" && login.Password == "Peter")
            {
                user = new UserModel {Username = "Peter McCullough", EmailAddress = "peter@gmail.com"};
            }
            return user;
        }
    }
}