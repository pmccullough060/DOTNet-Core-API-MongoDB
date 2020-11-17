using Newtonsoft.Json;

namespace MongoDBApi.Objects
{
    public class ErrorDetails 
    {
        public int StatusCode {get; set;}
        public string Message {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ErrorDetails Build(int StatusCode, string message)
        {
            var errorDetails = new ErrorDetails()
            {
                StatusCode = StatusCode,
                Message = message
            };

            return errorDetails;
        }
    }
}