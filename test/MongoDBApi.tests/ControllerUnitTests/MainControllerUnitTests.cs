using Microsoft.AspNetCore.Mvc;
using MongoDBApi.Controllers;
using MongoDBApi.CRUD;
using MongoDBApi.Objects;
using Moq;
using Xunit;

namespace MongoDBApi.tests.ControllerUnitTests
{
    public class ControllerUnitTests
    {
        public static string DbName = "Database";
        public static string ColName = "Collection";

        [Fact]
        public void DatabaseInfo_Exist_OkObjectResult()
        {
            var mockCRUD = new Mock<IMongoCRUDOps>();
            mockCRUD.Setup(x => x.GetAllDatabases()).Returns("SomeDatabaseInfo"); 
            var controller = new MainController(mockCRUD.Object);
            var actionResult = controller.DatabaseInfo();
            var contentResult = (OkObjectResult)actionResult;
            Assert.NotNull(contentResult);
            Assert.Equal(200, contentResult.StatusCode);
        }

        [Fact]
        public void DatabaseInfo_NoneExist_NotFoundObjectResult()
        {
            var mockCRUD = new Mock<IMongoCRUDOps>();
            mockCRUD.Setup(x => x.GetAllDatabases()).Returns(""); //returns an empty string same as MongoDB driver
            var controller = new MainController(mockCRUD.Object);
            var actionResult = controller.DatabaseInfo();
            var contentResult = (NotFoundObjectResult)actionResult;
            Assert.NotNull(contentResult);
            Assert.Equal(404, contentResult.StatusCode);
        }
        
        [Fact]
        public void CollectionInfo_CollectionExists_OkObjectResult()
        {
            var mockCRUD = new Mock<IMongoCRUDOps>();
            mockCRUD.Setup(x => x.GetAllCollections(DbName)).Returns(ColName); 
            var controller = new MainController(mockCRUD.Object);
            var actionResult = controller.CollectionInfo(DbName);
            var contentResult = (OkObjectResult)actionResult;
            Assert.NotNull(contentResult);
            Assert.Equal(200, contentResult.StatusCode);
        }

        [Fact]
        public void CollectionInfo_NoCollectionExists_NotFoundObjectResult()
        {
            var mockCRUD = new Mock<IMongoCRUDOps>();
            mockCRUD.Setup(x => x.CheckDatabaseExists(DbName)).Returns(true);
            var controller = new MainController(mockCRUD.Object);
            var actionResult = controller.CollectionInfo(DbName);
            var contentResult = (NotFoundObjectResult)actionResult;
            Assert.NotNull(contentResult);
            Assert.Equal(404, contentResult.StatusCode);
        }

        [Fact]
        public void ObjectInfo_CollectionExistsObjectsExist_OkObjectResult()
        {
            var mockCRUD = new Mock<IMongoCRUDOps>();
            mockCRUD.Setup(x => x.GetAllCollections(ColName)).Returns("someCollectionData");
            mockCRUD.Setup(x => x.GetFiles(DbName, ColName)).Returns("SomeObjectsData");
            var controller = new MainController(mockCRUD.Object);
            var actionResult = controller.ObjectInfo(DbName, ColName);
            var contentResult = (OkObjectResult)actionResult;
            Assert.NotNull(contentResult);
            Assert.Equal(200, contentResult.StatusCode);
        }

        [Fact]
        public void ObjectInfo_CollectionExistsNoObjectsExist_NotFoundObjectResult()
        {
            var mockCRUD = new Mock<IMongoCRUDOps>();
            mockCRUD.Setup(x => x.GetAllCollections(ColName)).Returns("SomeCollectionData");
            mockCRUD.Setup(x => x.GetFiles(DbName, ColName)).Returns("");
            var controller = new MainController(mockCRUD.Object);
            var actionResult = controller.ObjectInfo(DbName, ColName);
            var contentResult = (OkObjectResult)actionResult;
            Assert.NotNull(contentResult);
            Assert.Equal(200, contentResult.StatusCode);
        }
    }
}