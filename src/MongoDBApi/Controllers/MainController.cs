using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Authorize(Policy = "StandardUser")]
        public IActionResult DatabaseInfo()
        {
            var jsonStringDB =  _mongoCrudOps.GetAllDatabases();
            if(jsonStringDB != null)
                return Ok(jsonStringDB);
            else
                return NotFound(_errorDetails.Build(404, "No Database entries were found"));
        }

        [HttpGet("CollectionInfo")]
        public IActionResult CollectionInfo(string databaseName)
        {
            if(_mongoCrudOps.CheckDatabaseExists(databaseName) == false)
                return(NotFound(_errorDetails.Build(404, $"Database {databaseName} does not exist")));

           return Ok(_mongoCrudOps.GetAllCollections(databaseName));
        }

        [HttpGet("ObjectInfo")]
        public IActionResult ObjectInfo(string databaseName, string collectionName)
        {
            if(_mongoCrudOps.CheckDatabaseExists(databaseName) == false)
                return(NotFound(_errorDetails.Build(404, $"Database {databaseName} does not exist")));
            
            return Ok( _mongoCrudOps.GetFiles(databaseName, collectionName));
        }

        [HttpPost("CreateDatabase")]
        public IActionResult CreateDatabase(string databaseName)
        {
            if(_mongoCrudOps.CheckDatabaseExists(databaseName) == true)
                return(NotFound(_errorDetails.Build(422, $"Database {databaseName} already exists, please choose another name")));

            _mongoCrudOps.CreateNewDatabase(databaseName);
            return Ok($"Database: {databaseName} was created");
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> PostFile(List<IFormFile> files, string databaseName)
        {
            return Ok(await _mongoCrudOps.UploadFiles(files, databaseName));
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string fileName, string databaseName)
        {
            var filePath = await _mongoCrudOps.DownloadFile(fileName, databaseName);
            return PhysicalFile(filePath, "text/plain", fileName);
        }
    }
}