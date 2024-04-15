# WeatherDemo

.NET demo of weather application

**Time spent**: ~5 hours (30 minutes - this documentation page)
## Setup
To create test DB, you can use scripts from the "TestDB" directory.
 - Use the **dockerStart.ps1** to start the docker container with the MS SQL database.  The DB uses the "*YourStrong@Passw0rd*" password for the "*sa*" user.
 - Use the "**createDb.ps1**" to create an empty database with the name "*weather*"

Use the "**/src/MigrationUpdate.cmd**" to update your database with the last migrations.

 #### The connection string for the database you can set in the "appsettings.json" file

## Usage
API contains five methods:
 1. **GET**: /WeatherForecast - Get forecast for today
 2. **GET**: /WeatherForecast/foraday - Get forecast for a specific date
 3. **GET**: /WeatherForecast/foraweek - Get forecast for a week
 4. **POST**: /WeatherForecast - Set forecast for a specific date
 5. **DELETE**: /WeatherForecast - Delete forecast for a specific date

You can use the "**/swagger/index.html**" page to test all APIs.
 ![swagger page](/imgs/swagger.png)

> Use the "Web.UnitTest" project to work with unit tests.

## What is necessary to add and change:
 - Add caching to get data from a database.
 - Add authentication and authorization.
 - Change work with the connection string.
 - Add integration tests. 
 
---

 > Have a pleasant programming!