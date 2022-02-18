# Setup
You'll need dotnet core 6

To get all needed packages
```dotnet restore```


## Database creation
https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli

Install EF Core utils
```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Create Database
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```
