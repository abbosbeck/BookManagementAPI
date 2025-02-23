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
   dotnet ef database update

   ##Disclaimer:
This application implements JWT-based authentication, but it is not enforced at the endpoint level. Certain aspects could be improved—for example, storing users in a database instead of hard-coding them. However, given that authentication was an optional requirement and the deadline is approaching, the current implementation was chosen.
