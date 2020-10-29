using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Servers;
using MongoDB.Driver.GridFS;

namespace MongoDBApi.CRUD
{
    public class MongoCRUDOps : IMongoCRUDOps
    {
        public MongoClient EstablishClient(string connectionString)
        {
            try
            {
                var client = new MongoClient(connectionString);
                var dbNames = client.ListDatabaseNames().ToList();
                var database = client.GetDatabase(dbNames.First());
                database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(100);
                return client;
            }
            catch( Exception e)
            {
                throw new Exception("Unable to establish client using the provided connection string" + e);
            }
        }

        public bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            var mongoClient = EstablishClient(connectionString);
            var databaseNames = mongoClient.ListDatabaseNames().ToList();
            return (databaseNames.Contains(databaseName));
        }

        public string GetAllDatabases(string connectionString)
        {
            var mongoClient = EstablishClient(connectionString);
            var bsonDbList = mongoClient.ListDatabases().ToList();
            var jsonWriteSettings = new JsonWriterSettings {OutputMode = JsonOutputMode.CanonicalExtendedJson};
            return bsonDbList.ToJson();
        }

        public string GetFiles(string connectionString, string databaseName, string collectionName)
        {
            var mongoClient = EstablishClient(connectionString);
            var database = mongoClient.GetDatabase(databaseName);
            var fs = new GridFSBucket(database);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            var objects = collection.Find(new BsonDocument()).ToList();
            return objects.ToJson();
        }
    }

    



}