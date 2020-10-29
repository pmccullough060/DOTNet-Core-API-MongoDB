using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDBApi.CRUD;

namespace MongoDBApi.Controllers
{
    [Route("Commands")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMongoCRUDOps _mongoCrudOps;
        public MainController(IMongoCRUDOps mongoCRUDOps)
        {
            _mongoCrudOps = mongoCRUDOps;
        }

        [HttpGet("DatabaseInfo")]   
        public IActionResult DatabaseInfo(string connectionString) //mongodb://localhost:27017
        {
            var jsonStringDB =  _mongoCrudOps.GetAllDatabases(connectionString);
            if(jsonStringDB != null)
                return Ok(jsonStringDB);
            else
                return NotFound("No database entires found");
        }

    }
}