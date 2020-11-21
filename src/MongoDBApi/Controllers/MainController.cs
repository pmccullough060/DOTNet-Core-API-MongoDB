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

        public MainController(IMongoCRUDOps mongoCRUDOps)
        {
            _mongoCrudOps = mongoCRUDOps ?? throw new ArgumentNullException(nameof(mongoCRUDOps));
        }

        [HttpGet("DatabaseInfo")]   //Onus is on the client to ensure that the database exists, that is what this end point if for... that validation will not be performed in the other methods.
        [Authorize(Policy = "StandardUser")]
        public IActionResult DatabaseInfo()
        {
            var jsonStringDB =  _mongoCrudOps.GetAllDatabases();
            if(String.IsNullOrEmpty(jsonStringDB) == false)
                return Ok(jsonStringDB);
            else
                return NotFound("No Database entries were found");
        }

        [HttpGet("CollectionInfo")] //onus on caller to check if a collection exists using this api
        [Authorize(Policy = "StandardUser")]
        public IActionResult CollectionInfo(string databaseName)
        {
            var collectionInfo = _mongoCrudOps.GetAllCollections(databaseName);
            if(String.IsNullOrEmpty(collectionInfo) == false)
                return Ok(collectionInfo);
            else
                return NotFound("No collections were found");
        }

        [HttpGet("ObjectInfo")]
        [Authorize(Policy = "StandardUser")]
        public IActionResult ObjectInfo(string databaseName, string collectionName)
        {            
            var collectionInfo =_mongoCrudOps.GetAllCollections(databaseName);
            return Ok( _mongoCrudOps.GetFiles(databaseName, collectionName));
        }

        [HttpPost("CreateDatabase")] // this endpoint is nto relevant for atlas potentially delete...
        [Authorize(Policy = "StandardUser")]
        public IActionResult CreateDatabase(string databaseName)
        {
            if(_mongoCrudOps.CheckDatabaseExists(databaseName) == true)
                return(NotFound(ErrorDetails.Build(422, $"Database {databaseName} already exists, please choose another name")));

            _mongoCrudOps.CreateNewDatabase(databaseName);
            return Ok($"Database: {databaseName} was created");
        }

        [HttpPost("UploadFiles")]
        [Authorize(Policy = "StandardUser")]
        public async Task<IActionResult> PostFile(List<IFormFile> files, string databaseName)
        {
            return Ok(await _mongoCrudOps.UploadFiles(files, databaseName));
        }

        [HttpGet("DownloadFile")]
        [Authorize(Policy = "StandardUser")]
        public async Task<IActionResult> DownloadFile(string fileName, string databaseName)
        {
            var filePath = await _mongoCrudOps.DownloadFile(fileName, databaseName);
            return PhysicalFile(filePath, "text/plain", fileName);
        }
    }
}