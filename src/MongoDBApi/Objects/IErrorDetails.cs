namespace MongoDBApi.Objects
{
    public interface IErrorDetails
    {
        int StatusCode {get; set;}
        string Message {get; set;}

        string ToString();
        string Build(int statusCode, string message);
    }
}