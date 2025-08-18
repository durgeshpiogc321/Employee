# Employee Project Overview

## Solution Structure

This solution is organized into multiple projects, each with a clear responsibility:

- **Employee**: ASP.NET Core MVC web application (UI Layer)
- **Repositories**: Data access layer (DAL) for database operations
- **Services**: Business logic layer (BLL)
- **Shared**: Shared models, DTOs, and utility classes

---

## Project Details

### Employee (Web Application)
- **Purpose**: Main MVC application for managing employees
- **Key Folders/Files**:
  - `Controllers/`: MVC controllers (e.g., `EmployeeController`, `HomeController`)
  - `Views/`: Razor views for UI
  - `wwwroot/`: Static files (CSS, JS, images)
  - `IOC/`: Dependency injection setup
  - `Mapper/`: AutoMapper profiles
  - `Models/`: View models
  - `appsettings.json`: Configuration

### Repositories (Data Access Layer)
- **Purpose**: Handles database operations
- **Key Folders/Files**:
  - `DatabaseModels/`: Entity models (e.g., `Employee`)
  - `Repository/`: Repository implementations (e.g., `EmployeeRepository`)
  - `IRepository/`: Repository interfaces
  - `EmployeeDbContext.cs`: Entity Framework DbContext

### Services (Business Logic Layer)
- **Purpose**: Contains business logic and service interfaces
- **Key Folders/Files**:
  - `Services/`: Service implementations (e.g., `EmployeeService`)
  - `IServices/`: Service interfaces

### Shared (Common Models & Utilities)
- **Purpose**: Shared DTOs, models, and utility classes
- **Key Folders/Files**:
  - `Common/`: Utility classes (e.g., `ConfigurationKeys`, `Responses`)
  - `Models/`: Shared models

---

## Data Flow
1. **Controller** (Employee) receives HTTP request
2. **Service** (Services) processes business logic
3. **Repository** (Repositories) performs database operations
4. **Shared** models/DTOs are used for data transfer

---

## Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- AutoMapper
- Dependency Injection
- Razor Views

---

## Getting Started
1. Restore NuGet packages
2. Build the solution
3. Update database connection string in `appsettings.json`
4. Run the application

---

## Notes
- Follows clean architecture principles (separation of concerns)
- Easily extendable for new features
- Organized for maintainability and testability
