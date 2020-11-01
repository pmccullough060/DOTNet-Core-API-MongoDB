using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        void CreateNewDatabase(string databaseName);
        Task<string> UploadFiles(List<IFormFile> files, string databaseName);
        Task<string> DownloadFile(string fileName, string databaseName);
    }
}