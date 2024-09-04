
# Dev's Notes

## Setup
```bash
mkdir Eshop
cd Eshop
dotnet new sln -n Eshop
dotnet new webapi -n EshopApi
dotnet new xunit -n EshopApiTest
dotnet sln add ./EshopApi/EshopApi.csproj
dotnet sln add ./EshopApiTest/EshopApiTest.csproj
dotnet sln list
```

## Eshop API
```bash
cd EshopApi
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Asp.Versioning.Mvc
dotnet add package Asp.Versioning.Mvc.ApiExplorer
dotnet add package DotNetEnv
# Update code: Domain/Infrastructure/Application/API
# Follow template https://github.com/nhonvo/clean-architecture-net-8.0
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
# dotnet ef database drop
# dotnet ef migrations remove
dotnet ef migrations add InitialCreate -o Infrastructure/Migrations
dotnet ef database update
dotnet run
```

## Eshop API Test
```bash
# Update code
dotnet test
```

## TODO
- [ ] Filter
