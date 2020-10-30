using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDBApi.CRUD;
using MongoDBApi.Objects;

namespace MongoDBApi.Controllers
{
    [Route("Commands")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMongoCRUDOps _mongoCrudOps;
        private readonly IErrorDetails _errorDetails;
        public MainController(IMongoCRUDOps mongoCRUDOps, IErrorDetails errorDetails)
        {
            _mongoCrudOps = mongoCRUDOps ?? throw new ArgumentNullException(nameof(mongoCRUDOps));
            _errorDetails = errorDetails ?? throw new ArgumentNullException(nameof(errorDetails));
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

        [HttpGet("CollectionInfo")]
        public IActionResult CollectionInfo(string connectionString, string databaseName)
        {
            if(_mongoCrudOps.CheckDatabaseExists(connectionString, databaseName) == false)
                return(NotFound(_errorDetails.Build(404, $"Database {databaseName} does not exist")));

           return Ok(_mongoCrudOps.GetAllCollections(connectionString, databaseName));
        }

        [HttpGet("ObjectInfo")]
        public IActionResult ObjectInfo(string connectionString, string databaseName, string collectionName)
        {
            if(_mongoCrudOps.CheckDatabaseExists(connectionString, databaseName) == false)
                return(NotFound(_errorDetails.Build(404, $"Database {databaseName} does not exist")));

            return Ok( _mongoCrudOps.GetFiles(connectionString, databaseName, collectionName));
        }




    }
}