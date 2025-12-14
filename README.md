# Product Management API

A RESTful API built with ASP.NET Core for managing products, product types, and colours with many-to-many relationships.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Testing with Swagger](#testing-with-swagger)
- [Troubleshooting](#troubleshooting)

## Prerequisites

Before setting up the project, ensure you have the following installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [DB Browser for SQLite](https://sqlitebrowser.org/) (optional, for database inspection)
- Git (for version control)

## Project Structure
Products/ ├── API/                          # Web API layer │   ├── Controllers/              # API controllers │   ├── appsettings.json          # Application configuration │   └── Program.cs                # Application entry point ├── Business/                     # Business logic layer │   ├── Handlers/                 # Business logic handlers │   └── Interfaces/               # Business interfaces │       ├── Handler/              # Handler interfaces │       └── Repository/           # Repository interfaces ├── Domain/                       # Domain models │   ├── EntityModel/              # Database entities │   ├── Request/                  # Request DTOs │   └── Response/                 # Response DTOs └── Infrastructure/               # Data access layer └── Data/                     # DbContext and repositories

## Technology Stack

- **Framework**: .NET 10
- **Language**: C# 14.0
- **Database**: SQLite
- **ORM**: Entity Framework Core
- **API Documentation**: Swagger/OpenAPI
- **Architecture**: Clean Architecture (Layered)

## Getting Started

### 1. Clone the Repository
git clone <your-repository-url> cd Products

### 2. Restore NuGet Packages

dotnet restore

### 3. Build the Solution
dotnet build

## Database Setup

### Connection String Configuration

The application uses SQLite as the database. Configure the connection string in `appsettings.json`:
{ "ConnectionStrings": { "DefaultConnection": "Data Source=app.db" } }

**Note**: The database file will be created in the API project's execution directory (typically `API/bin/Debug/net10.0/app.db`).

### Database Migrations

If using Entity Framework migrations:

Navigate to the API project directory
cd API
Create a new migration
dotnet ef migrations add InitialCreate --project ../Infrastructure --startup-project .

Apply migrations to create the database
dotnet ef database update --project ../Infrastructure --startup-project .

### Seed Sample Data (Optional)

To populate the database with sample data, you can:

1. **Use the DbSeeder class** (if implemented):
   - The seeder runs automatically on application startup
   - Sample colours and product types will be inserted

2. **Use SQL directly**:
   - Open the database file in DB Browser for SQLite
   - Run the following SQL:
-- Insert sample colours INSERT INTO Colours (ColourName, CreatedBy, CreatedDate) VALUES ('Red', 'system', datetime('now')), ('Blue', 'system', datetime('now')), ('Green', 'system', datetime('now')), ('Black', 'system', datetime('now')), ('White', 'system', datetime('now'));
-- Insert sample product types INSERT INTO ProductTypes (ProductTypeName, CreatedBy, CreatedDate) VALUES ('Electronics', 'system', datetime('now')), ('Clothing', 'system', datetime('now')), ('Furniture', 'system', datetime('now')), ('Books', 'system', datetime('now'));
	
	- 
## Configuration

### appsettings.json
{ "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } }, "AllowedHosts": "*", "ConnectionStrings": { "DefaultConnection": "Data Source=app.db" } }

### Dependency Injection Setup

Ensure services are registered in `Program.cs`:
// Add DbContext builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Register repositories builder.Services.AddScoped<IProductRepository, ProductRepository>(); builder.Services.AddScoped<IColourRepository, ColourRepository>(); builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
// Register handlers builder.Services.AddScoped<IProductHandler, ProductHandler>(); builder.Services.AddScoped<IColourHandler, ColourHandler>(); builder.Services.AddScoped<IProductTypeHandler, ProductTypeHandler>();

## Running the Application

### Using Visual Studio

1. Set `API` project as the startup project
2. Press `F5` or click the "Run" button
3. The application will launch in your default browser

### Using Command Line

Navigate to the API project
cd API
Run the application
dotnet run

### Using IIS Express (Visual Studio)

1. Open the solution in Visual Studio
2. Select "IIS Express" from the debug dropdown
3. Press `F5`

The API will be available at:
- HTTPS: `https://localhost:7xxx`
- HTTP: `http://localhost:5xxx`

(Port numbers may vary based on your `launchSettings.json`)

## API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/product` | Get all products |
| GET | `/api/product/{id}` | Get product by ID |
| POST | `/api/product` | Create a new product |

**Sample POST Request:**
{ "name": "Laptop", "productTypeId": 1, "colourIds": [1, 2], "createdBy": "admin" }

### Colours

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/colour` | Get all colours |
| POST | `/api/colour` | Create a new colour |

**Sample POST Request:**
{ "colourName": "Yellow" }

### Product Types

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/producttype` | Get all product types |
| POST | `/api/producttype` | Create a new product type |

**Sample POST Request:**
{ "productTypeName": "Appliances" }

## Testing with Swagger

The API includes Swagger UI for easy testing and documentation.

### Accessing Swagger

1. Run the application
2. Navigate to: `https://localhost:xxxx/swagger`
3. The Swagger UI will display all available endpoints

### Testing Workflow

1. **Create Colours**:
   - Expand `POST /api/colour`
   - Click "Try it out"
   - Enter colour data
   - Click "Execute"
   - Note the returned ID

2. **Create Product Types**:
   - Expand `POST /api/producttype`
   - Click "Try it out"
   - Enter product type data
   - Click "Execute"
   - Note the returned ID

3. **Create Products**:
   - Expand `POST /api/product`
   - Use the IDs from colours and product types created above
   - Enter product data with valid `productTypeId` and `colourIds`
   - Click "Execute"

4. **Retrieve Data**:
   - Use GET endpoints to verify created data

## Troubleshooting

### Foreign Key Constraint Failed

**Error**: `SQLite Error 19: 'FOREIGN KEY constraint failed'`

**Cause**: Trying to create a product with non-existent `ProductTypeId` or `ColourIds`.

**Solution**:
1. Verify that the ProductType exists: `GET /api/producttype`
2. Verify that all Colours exist: `GET /api/colour`
3. Use valid IDs from the responses

### Cannot Query Database

**Issue**: SQLite browser shows 0 rows even after inserting data.

**Causes & Solutions**:

1. **Wrong database file**: 
   - Check the actual database location by adding logging:
	var dbPath = _context.Database.GetDbConnection().DataSource; Console.WriteLine($"Database: {dbPath}");
	-    - Open the exact file path shown in the console

2. **Application still running**:
   - Stop the application before querying the database
   - SQLite locks the file during active connections

3. **Multiple database files**:
   - Search your solution folder for all `.db` files
   - Check file modification timestamps to find the active one

### Swagger Validation Error

**Error**: "Required field is not provided" for route parameters

**Solution**: Ensure route parameters use type constraints:
[HttpGet("{id:int}")]

### Application Won't Start

1. **Check port conflicts**: 
   - Another application may be using the configured port
   - Modify ports in `launchSettings.json`

2. **Missing dependencies**:
 dotnet restore dotnet build

3. **Database connection**:
   - Verify connection string in `appsettings.json`
   - Ensure SQLite provider is installed

### Entity Framework Issues

**Migrations not found**:
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API

**Database not updating**:
dotnet ef database update --project Infrastructure --startup-project API

## Development Tips

### Viewing Database Contents

1. **Using DB Browser for SQLite**:
   - Open the database file: `API/bin/Debug/net10.0/app.db`
   - Browse data in the "Browse Data" tab

2. **Using Visual Studio**:
   - View → SQL Server Object Explorer
   - Add SQLite connection (may require extension)

### Debugging

1. Set breakpoints in your code
2. Use the debugger to inspect:
   - Request payloads
   - Entity states
   - Database queries (check Output window → Debug)

### Logging Database Queries

Enable sensitive data logging in development:
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")) .EnableSensitiveDataLogging() .LogTo(Console.WriteLine, LogLevel.Information));

## Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [Swagger/OpenAPI Specification](https://swagger.io/specification/)

## License

Test Project

## Contributors

Appala naidu Kolli/Test

