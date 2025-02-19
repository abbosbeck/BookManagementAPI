# Book Management API

This is a RESTful API built with ASP.NET Core and EF Core for managing books.  
It supports CRUD operations, tracks the number of times a book is viewed, and calculates a popularity score.

## Requirements
- .NET 8 or 9
- SQL Server

## Getting Started
1. Clone the repo
2. Update the connection string in appsettings.json
3. Run migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
