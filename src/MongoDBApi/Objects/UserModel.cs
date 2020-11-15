using System;
using Newtonsoft.Json;

namespace MongoDBApi.Objects
{
    public class UserModel : IUserModel
    {
        public string Username {get; set;}
        public string Password {get; set;}
        public string EmailAddress {get; set;}
        public DateTime DateOfJoining {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}