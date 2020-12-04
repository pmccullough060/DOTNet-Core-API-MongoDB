using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MongoDBApi.Objects;
using System.Collections.Generic;

namespace MongoDBApi.AuthClasses
{
    public class Auth : IAuth
    {
        private readonly IConfiguration _config;

        public Auth(IConfiguration config)
        {
            _config = config;
        }

        //in the user model we pass in an argument that relays the authorization level....
        public string GenerateJSONWebToken(UserModel userInfo)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                new Claim("DateOfJoining", userInfo.DateOfJoining.ToString("yyyy-MM-dd")),
                new Claim("IDNumber", "1"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if(userInfo.AuthorisationLevel.Equals(AuthLevel.Administrator))
            {
                claims.Add(new Claim(ClaimTypes.Role, nameof(AuthLevel.Administrator)));
            }
            else if(userInfo.AuthorisationLevel.Equals(AuthLevel.StandardUser))
            {
                claims.Add(new Claim(ClaimTypes.Role, nameof(AuthLevel.StandardUser)));
            }

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["JWT:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UserModel AuthenticateUser(UserModel login) //just for testing this can be expanded to allow database storage of users etc.
        {
            UserModel user = null;

            if(login.Username == "Peter" && login.Password == "Peter")
            {
                user = new UserModel()
                {
                    Username = "PeterAdministrator", 
                    EmailAddress= "PeterAdministrator@gmail.com",
                    AuthorisationLevel = AuthLevel.StandardUser
                };
            }

            else if(login.Username == "SuperPeter" && login.Password == "SuperPeter")
            {
                user = new UserModel()
                {
                    Username = "SuperPeterAdministrator", 
                    EmailAddress= "SuperPeterAdministrator@gmail.com",
                    AuthorisationLevel = AuthLevel.Administrator
                };
            }

            return user;
        }
    }
}