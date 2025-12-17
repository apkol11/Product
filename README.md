# Product Management API Backend
 
Welcome to the Product Management API Backend repository! This project provides a RESTful API built with ASP. NET Core for managing products, product types, and colours with many-to-many relationships.
 
## Table of Contents
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
  - [Step 1: Clone the Repository](#step-1-clone-the-repository)
  - [Step 2: Configure the Database Connection](#step-2-configure-the-database-connection)
  - [Step 3: Restore Dependencies and Build](#step-3-restore-dependencies-and-build)
  - [Step 4: Database Setup](#step-4-database-setup)
- [Usage](#usage)
  - [Running the Application](#running-the-application)
  - [Swagger](#swagger)
- [API Endpoints](#api-endpoints)
- [Solution Structure](#solution-structure)
- [Troubleshooting](#troubleshooting)
- [License](#license)
 
## Features
- RESTful API with ASP.NET Core
- Entity Framework Core for data access with SQLite
- Swagger for API documentation
- Dependency injection for services
- Clean Architecture (Layered approach)
- Many-to-many relationships between Products, Product Types, and Colours
 
## Tech Stack
- C# 14.0
- ASP.NET Core
- Entity Framework Core
- SQLite
- Swagger/OpenAPI
 
## Prerequisites
- [. NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) (or [Visual Studio Code](https://code.visualstudio.com/) with C# extensions)
- [DB Browser for SQLite](https://sqlitebrowser.org/) (optional, for database inspection)
- Git for version control
 
## Installation

### Step 1: Clone the Repository
```bash
git clone <your-repository-url>
cd Products
```
 
### Step 2: Configure the Database Connection
 
Open `appsettings.json` in the API project and configure the connection string for the database.
 
The application uses SQLite, and the database file will be created in the API project's execution directory (typically `API/bin/Debug/net10.0/app.db`).
 
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings":  {
    "DefaultConnection": "Data Source=app.db"
  }
}
```

### Step 3: Restore Dependencies and Build

Restore NuGet packages:
```bash
dotnet restore
```

Build the solution:
```bash
dotnet build
```

### Step 4: Database Setup

Navigate to the API project directory: 
```bash
cd API
```

Create a new migration (if using Entity Framework migrations):
```bash
dotnet ef migrations add InitialCreate --project ../Infrastructure --startup-project .
```

Apply migrations to create the database:
```bash
dotnet ef database update --project ../Infrastructure --startup-project .
```

#### Seed Sample Data (Optional)

To populate the database with sample data:

1. **Use the DbSeeder class** (if implemented):
   - The seeder runs automatically on application startup
   - Sample colours and product types will be inserted

2. **Use SQL directly**: 
   - Open the database file in DB Browser for SQLite
   - Run SQL to insert sample data:
   
```sql
-- Insert sample colours
INSERT INTO Colours (ColourName, CreatedBy, CreatedDate) 
VALUES 
  ('Red', 'system', datetime('now')),
  ('Blue', 'system', datetime('now')),
  ('Green', 'system', datetime('now'));

-- Insert sample product types
INSERT INTO ProductTypes (ProductTypeName, CreatedBy, CreatedDate) 
VALUES 
  ('Electronics', 'system', datetime('now')),
  ('Clothing', 'system', datetime('now')),
  ('Furniture', 'system', datetime('now'));
```
 
## Usage
 
### Running the Application
 
#### Command Line

**Navigate to the API project directory:**
```bash
cd API
```

Start the application:
```bash
dotnet run
```

Start the application with hot reload:
```bash
dotnet watch run
```

#### Visual Studio
1. Open the solution in Visual Studio
2. Set `API` project as the startup project
3. Press `Ctrl + F5` (or `F5` to run with the debugger) to start the application

#### Using IIS Express
1. Open the solution in Visual Studio
2. Select "IIS Express" from the debug dropdown
3. Press `F5`

The API will be available at:
- HTTPS: `https://localhost:7xxx`
- HTTP: `http://localhost:5xxx`

(Port numbers may vary based on your `launchSettings.json`)
 
### Swagger
 
The API includes Swagger UI for easy testing and documentation.

- Open the Swagger UI: `https://localhost:xxxx/swagger`
- The Swagger UI will display all available endpoints with interactive documentation

#### Testing Workflow

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

## API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/product` | Get all products |
| GET | `/api/product/{id}` | Get product by ID |
| POST | `/api/product` | Create a new product |

**Sample POST Request:**
```json
{
  "name": "Laptop",
  "productTypeId": 1,
  "colourIds": [1, 2],
  "createdBy": "admin"
}
```

### Colours

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/colour` | Get all colours |
| POST | `/api/colour` | Create a new colour |

**Sample POST Request:**
```json
{
  "colourName": "Yellow"
}
```

### Product Types

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/producttype` | Get all product types |
| POST | `/api/producttype` | Create a new product type |

**Sample POST Request:**
```json
{
  "productTypeName":  "Appliances"
}
```

## Solution Structure
 
```sh
Products/
├── 📁 API                          # Application entry point, Web API layer
│   ├── Controllers/                # API controllers
│   ├── appsettings.json           # Application configuration
│   └── Program.cs                 # Application startup
│
├── 📁 Application                  # Application layer with business logic
│   ├── Handlers/                  # Request handlers
│   └── DTOs/                      # Data transfer objects
│
├── 📁 Domain                       # Core business entities and logic
│   ├── Entities/                  # Domain entities
│   └── Interfaces/                # Repository interfaces
│
└── 📁 Infrastructure               # Data access and external services
    ├── Data/                      # Database context
    ├── Repositories/              # Repository implementations
    └── Migrations/                # EF Core migrations
```

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
   ```csharp
   var dbPath = _context.Database.GetDbConnection().DataSource;
   Console.WriteLine($"Database: {dbPath}");
   ```
   - Open the exact file path shown in the console

2. **Application still running**: 
   - Stop the application before querying the database
   - SQLite locks the file during active connections

3. **Multiple database files**: 
   - Search your solution folder for all `.db` files
   - Check file modification timestamps to find the active one

### Application Won't Start

1. **Check port conflicts**: 
   - Another application may be using the configured port
   - Modify ports in `launchSettings.json`

2. **Missing dependencies**: 
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Database connection**:
   - Verify connection string in `appsettings.json`
   - Ensure SQLite provider is installed

### Entity Framework Issues

**Migrations not found**: 
```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
```

**Database not updating**:
```bash
dotnet ef database update --project Infrastructure --startup-project API
```

### Development Tips

#### Viewing Database Contents

1. **Using DB Browser for SQLite**:
   - Open the database file: `API/bin/Debug/net10.0/app.db`
   - Browse data in the "Browse Data" tab

2. **Using Visual Studio**:
   - View → SQL Server Object Explorer
   - Add SQLite connection (may require extension)

#### Debugging

1. Set breakpoints in your code
2. Use the debugger to inspect: 
   - Request payloads
   - Entity states
   - Database queries (check Output window → Debug)

#### Logging Database Queries

Enable sensitive data logging in development:
```csharp
builder.Services. AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information));
```

## Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [Swagger/OpenAPI Specification](https://swagger.io/specification/)

## License

Test Project

## Contributors

Appala Naidu Kolli
