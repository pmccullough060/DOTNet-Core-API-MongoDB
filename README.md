# DOTNet Core API MongoDB
This project was intially created as a simple Web API for temporary storage (caching) of CAD files as Blobs (Binary Large Objects) in a MongoDB Atlas cloud Database. I was curious to see if there was a benefit over a traditional FTP + RDBMS (for storing filepaths) setup in the following areas:
* Quicker to retrieve files than FTP + RDBMS
* Easier to setup, backup and scale 
* Easier revision management

## Getting Started
Either Visual Studio Code or Visual Studio work great, for the database side of things you'll need to:
1. Install [MongoDBCompass](https://www.mongodb.com/products/compass) 
2. (Optional) Create a [MongoDB Atlas](https://www.mongodb.com/cloud/atlas) Atlas Cloud cluster.

Once you have these setup you need to edit your appsettings.json file accordingly

1. For MongoDB Atlas:

```json
{
  "DatabaseSettings": {
    "connectionString": "mongodb+srv://YourUserName:YourPassword@cluster0.dqsmb.mongodb.net/test?retryWrites=true&w=majority"
}
```
2. For a local instance of MongoDBCompass
```json
{
  "DatabaseSettings": {
    "connectionString": "mongodb://localhost:27017"
}
```

### Prerequisites
