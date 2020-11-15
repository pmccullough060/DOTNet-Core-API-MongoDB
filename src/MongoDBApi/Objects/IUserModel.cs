using System;

namespace MongoDBApi.Objects
{
    public interface IUserModel
    {
        string Username {get; set;}
        string Password {get; set;}
        string EmailAddress {get; set;}
        DateTime DateOfJoining {get; set;}

        string ToString();
    }
}