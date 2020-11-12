using System;

namespace MongoDBApi.Objects
{
    public class UserModel
    {
        public string Username {get; set;}
        public string Password {get; set;}
        public string EmailAddress {get; set;}
        public DateTime DateOfJoining {get; set;}
    }
}