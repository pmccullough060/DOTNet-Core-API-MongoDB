using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDBApi.CRUD;
using MongoDBApi.Objects;

namespace MongoDBApi.Controllers
{
    [Route("Commands")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMongoCRUDOps _mongoCrudOps;
        private readonly ILogger<MainController> _logger;

        public MainController(IMongoCRUDOps mongoCRUDOps, ILogger<MainController> logger)
        {
            _mongoCrudOps = mongoCRUDOps ?? throw new ArgumentNullException(nameof(mongoCRUDOps));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("DatabaseInfo")]   //Onus is on the client to ensure that the database exists, that is what this end point if for... that validation will not be performed in the other methods.
        [Authorize(Policy = "StandardUser")]
        public IActionResult DatabaseInfo()
        {
            _logger.LogInformation("Processing DatabaseInfo request");

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
        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> PostFile(List<IFormFile> files, string databaseName)
        {
            return Ok(await _mongoCrudOps.UploadFiles(files, databaseName));
        }

        [HttpGet("DownloadFile")]
        [Authorize(Policy = "StandardUser")]
        public async Task<IActionResult> DownloadFile(string fileName, string databaseName)
        {
            var stream = await _mongoCrudOps.DownloadFile(fileName, databaseName);
            return File(stream, "application/....", fileName);
        }
    }
}