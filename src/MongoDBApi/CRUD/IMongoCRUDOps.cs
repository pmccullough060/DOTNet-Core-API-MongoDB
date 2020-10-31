using MongoDB.Driver;

namespace MongoDBApi.CRUD
{
    public interface IMongoCRUDOps
    {
        MongoClient EstablishClient();
        bool CheckDatabaseExists(string databaseName);
        string GetAllDatabases();
        string GetFiles(string databaseName, string collectionName);
        string GetAllCollections(string databaseName);
    }
}