using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Servers;
using MongoDB.Driver.GridFS;
using MongoDBApi.CustomExceptions;
using MongoDBApi.Database;
using MongoDBApi.Objects;

namespace MongoDBApi.CRUD
{
    public class MongoCRUDOps : IMongoCRUDOps
    {
        private readonly IDatabaseSettings _databaseSettings;
        private readonly IUploadData _uploadData;
        private readonly MongoClient client;

        public MongoCRUDOps(IDatabaseSettings databaseSettings, IUploadData uploadData)
        {
            _databaseSettings = databaseSettings ?? throw new ArgumentNullException(nameof(databaseSettings));
            client = EstablishClient();
            _uploadData = uploadData;
        }

        public MongoClient EstablishClient()
        {
            try
            {
                var client = new MongoClient(_databaseSettings.connectionString);
                var dbNames = client.ListDatabaseNames().ToList();
                var database = client.GetDatabase(dbNames.First());
                database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(100);
                return client;
            }
            catch (Exception e)
            {
                throw new NoDBConnectionException("Cannot establish DB connection" + e);
            }
        }

        public bool CheckDatabaseExists(string databaseName)
        {
            var databaseNames = client.ListDatabaseNames().ToList();
            return (databaseNames.Contains(databaseName));
        }

        public string GetAllDatabases()
        {
            var bsonDbList = client.ListDatabases().ToList();                
            var jsonWriteSettings = new JsonWriterSettings {OutputMode = JsonOutputMode.CanonicalExtendedJson};
            return bsonDbList.ToJson();
        }

        public string GetAllCollections(string databaseName)
        {
            var database = client.GetDatabase(databaseName);
            var collections = database.ListCollectionNames().ToList();
            if (!collections.Any())
                throw new ArgumentException();
            var jsonWriteSettings = new JsonWriterSettings {OutputMode = JsonOutputMode.CanonicalExtendedJson};
            return collections.ToJson();
        }

        public string GetFiles(string databaseName, string collectionName)
        {
            var database = client.GetDatabase(databaseName);
            var fs = new GridFSBucket(database);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            var objects = collection.Find(new BsonDocument()).ToList();
            return objects.ToJson();
        }

        public void CreateNewDatabase(string databaseName)
        {
            try
            {
                var database = client.GetDatabase(databaseName);
                database.CreateCollection(databaseName);
            }
            catch(Exception e)
            {
                throw new Exception("Unable to create Database" + e); //may need to create a custom exception and catch in the middleware exception handling.
            }
        }

        public async Task<string> UploadFiles(List<IFormFile> files, string databaseName)
        {
            var database = client.GetDatabase(databaseName);
            var fs = new GridFSBucket(database);
            long size = files.Sum(x => x.Length);
            var filePaths = new List<string>();
            foreach(var file in files)
            {
                if(file.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    filePaths.Add(filePath);
                    using(var stream = File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                    using(var stream = new FileStream(filePath, FileMode.Open))
                    {
                        await fs.UploadFromStreamAsync(file.FileName, stream);
                    }
                }
            }
            return _uploadData.Build(files.Count(), size, filePaths);
        }

        public async Task<string> DownloadFile(string fileName, string databaseName)
        {
            var database = client.GetDatabase(databaseName);
            var fs = new GridFSBucket(database);
            var filePath = Path.GetTempFileName();
            using(var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                await fs.DownloadToStreamByNameAsync(fileName, stream);
            }
            return filePath;
        }
    }
}