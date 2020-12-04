# DOTNet Core API MongoDB
This project was intially created as a simple Web API for temporary storage (caching) of CAD files as Blobs (Binary Large Objects) in a MongoDB Atlas cloud Database. I was curious to see if there was a benefit over a traditional FTP + RDBMS (for storing filepaths) setup in the following areas:
* Quicker to retrieve files than FTP + RDBMS
* Easier to setup, backup and scale 
* Easier revision management

## Getting Started
Either Visual Studio Code or Visual Studio work great or use your Text editor/IDE of choice, for the database side of things you'll need to:
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
Thats it! all setup and running locally. 

## Design Notes 

### Authentication and Authorization
To begin using the API end points a user must first login with the correct username and password crendentials, a JWT token will then be issued for the corresponding authorization level. By default this API has two authorization levels, StandardUser and Administrator. Only an Adminstrator is allowed to upload files.

To retrieve your JWT token, send an HTTP Post Request to /Authentication/Login with the Json file in the request body shown below:

```json
{
  "username": "blah"
  "password": "blah"
}
```

