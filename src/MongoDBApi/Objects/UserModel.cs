using System;
using Newtonsoft.Json;

namespace MongoDBApi.Objects
{
    public enum AuthLevel
    {
        StandardUser,
        Administrator
    }

    public class UserModel : IUserModel
    {
        public string Username {get; set;}
        public string Password {get; set;}
        public string EmailAddress {get; set;}
        public DateTime DateOfJoining {get; set;}
        public AuthLevel AuthorisationLevel {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}