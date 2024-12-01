# Cosmos Odyssey

The web application is deployed on Render and available [here](https://cosmosodyssey-4zyi.onrender.com). Since the application needs to cold start, it may take a minute or two to load.  

The application is built with .NET 8.0 and MongoDB. The frontend utilizes Razor Pages, with some JavaScript used for filtering reservations and toggling the flight card dropdowns on the reservation page. The application is hosted on Render using docker, while the database is hosted on MongoDB Atlas.
## Setup to run the project locally
In order to run the project locally, you need to add MongoDb connection string and database name in `appsettings.json` file that is located in WebApp project folder.
To get the connection string and database name, you need to create a database. A short guide on how to create a database is available [here](https://www.mongodb.com/resources/products/platform/mongodb-atlas-tutorial#create-a-mongodb-cloud-account).
After inserting the connection string and database name, you can run the WebApp project. 
