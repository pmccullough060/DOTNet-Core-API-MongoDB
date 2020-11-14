using MongoDBApi.Objects;

namespace MongoDBApi.AuthClasses
{
    public interface IAuth
    {
        string GenerateJSONWebToken(UserModel userInfo);

        UserModel AuthenticateUser(UserModel login);
    }
}