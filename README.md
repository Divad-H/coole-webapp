# Coole Webapp

![build workflow](https://github.com/Divad-H/coole-webapp/actions/workflows/main_coole-webapp.yml/badge.svg)

## Database

### Creating Migrations

From within the database project directory call `dotnet ef migrations add [MigrationName] --startup-project ..\CooleWebapp.Backend\CooleWebapp.Backend.csproj`

### Browsing the Local Database in Visual Studio

* Open the Server Explorer
* Add a new Data Connection
* Use the Microsoft SQL Server provider
* Run SqlLocalDb.exe info
* Run SqlLocalDb.exe start MSSQLLocalDB or the desired name
* Run SqlLocalDb.exe info MSSQLLocalDB
* Copy the instance pipe name `np:\\.\pipe\LOCALDB#030934C6\tsql\query` into the server name
* Choose the master database 
