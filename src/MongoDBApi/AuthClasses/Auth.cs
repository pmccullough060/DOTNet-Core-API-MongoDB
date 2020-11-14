using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MongoDBApi.Objects;

namespace MongoDBApi.AuthClasses
{
    public class Auth : IAuth
    {
        private readonly IConfiguration _config;

        public Auth(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJSONWebToken(UserModel userInfo)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                new Claim("DateOfJoining", userInfo.DateOfJoining.ToString("yyyy-MM-dd")),
                new Claim("IDNumber", "1"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["JWT:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            if(login.Username == "Peter" && login.Password == "Peter")
            {
                user = new UserModel {Username = "Peter McCullough", EmailAddress= "Peter@gmail.com"};
            }

            return user;
        }
    }
}