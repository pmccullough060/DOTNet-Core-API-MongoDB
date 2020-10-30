using MongoDB.Driver;

namespace MongoDBApi.CRUD
{
    public interface IMongoCRUDOps
    {
        MongoClient EstablishClient(string connectionString);
        bool CheckDatabaseExists(string connectionString, string databaseName);
        string GetAllDatabases(string connectionString);
        string GetFiles(string connectionString, string databaseName, string collectionName);
        string GetAllCollections(string connectionString, string databaseName);
    }
}