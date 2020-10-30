using Newtonsoft.Json;

namespace MongoDBApi.Objects
{
    public class ErrorDetails : IErrorDetails
    {
        public int StatusCode {get; set;}
        public string Message {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Build(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}