using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        Task<MemoryStream> DownloadFile(string fileName, string databaseName);
    }
}