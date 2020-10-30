namespace MongoDBApi.Objects
{
    public interface IErrorDetails
    {
        int StatusCode {get; set;}
        string Message {get; set;}

        string ToString();
        void Build(int statusCode, string message);
    }
}