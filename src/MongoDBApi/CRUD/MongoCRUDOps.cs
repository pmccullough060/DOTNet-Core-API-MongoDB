using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Servers;
using MongoDB.Driver.GridFS;
using MongoDBApi.CustomExceptions;
using MongoDBApi.Database;

namespace MongoDBApi.CRUD
{
    public class MongoCRUDOps : IMongoCRUDOps
    {
        private readonly IDatabaseSettings _databaseSettings;
        private readonly MongoClient client;

        public MongoCRUDOps(IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings ?? throw new ArgumentNullException(nameof(databaseSettings));
            client = EstablishClient();
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
            catch
            {
                throw new NoDBConnectionException();
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
    }
}