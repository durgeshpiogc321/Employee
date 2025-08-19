# Copilot Coding Instructions

This document outlines the coding standards and best practices for the Employee project. All contributors and AI code generation tools (such as GitHub Copilot) should follow these guidelines to ensure code consistency, readability, and maintainability.

---

## Project Overview

### Solution Architecture
This solution follows clean architecture principles with clear separation of concerns:

- **Employee**: ASP.NET Core MVC web application (UI Layer)
- **Repositories**: Data access layer (DAL) for database operations
- **Services**: Business logic layer (BLL)
- **Shared**: Shared models, DTOs, and utility classes

### Data Flow
1. **Controller** (Employee) receives HTTP request
2. **Service** (Services) processes business logic
3. **Repository** (Repositories) performs database operations
4. **Shared** models/DTOs are used for data transfer

### Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- AutoMapper
- Dependency Injection
- Razor Views

---

## Comprehensive Coding Standards

### Architecture Principles
- **Separation of Concerns**: Each layer has distinct responsibilities
- **Dependency Injection**: Use DI for loose coupling
- **Repository Pattern**: Database operations through repositories
- **Service Layer**: Business logic isolated from controllers
- **Clean Code**: Readable, maintainable, and testable code

### Security Standards
- **Input Validation**: Validate all user inputs
- **SQL Injection Prevention**: Use parameterized queries
- **XSS Protection**: Encode outputs and validate inputs
- **Authentication/Authorization**: Proper access controls
- **Error Handling**: No sensitive information in error messages
- **Logging**: Log security events and errors appropriately

### Performance Guidelines
- **Async/Await**: Use async patterns for I/O operations
- **Database Efficiency**: Optimize queries and use proper indexing
- **Memory Management**: Dispose resources properly
- **Caching**: Implement caching where appropriate
- **Load Testing**: Test performance under load

---

## Code Style

- **Use semantic HTML5 elements**  
  When writing frontend code, prefer semantic HTML5 tags such as `header`, `main`, `section`, `article`, etc., to improve accessibility and structure.

- **Prefer modern JavaScript (ES6+) features**  
  Use `const` and `let` instead of `var`, arrow functions, template literals, destructuring, and other ES6+ features for cleaner and more robust code.

---

## Naming Conventions

- **PascalCase**  
  Use PascalCase for component names, interfaces, and type aliases.  
  _Example: `EmployeeService`, `IEmployeeRepository`_

- **camelCase**  
  Use camelCase for variables, functions, and methods.  
  _Example: `getAllEmployees`, `employeeRepository`_

- **Prefix private class members with underscore**  
  _Example: `_mapper`_

- **ALL_CAPS for constants**  
  _Example: `MAX_EMPLOYEE_COUNT`_

---

## Code Quality

- **Use meaningful names**  
  Choose variable and function names that clearly describe their purpose.

- **Comment complex logic**  
  Add helpful comments to explain non-trivial code sections.

- **Error handling**  
  Always add error handling for user inputs and API calls to ensure robustness.

- **No debug code**  
  Remove console.log, debug variables, and commented-out code before committing.

- **Code formatting**  
  Use consistent indentation and follow C# formatting conventions.

---

## Layer-Specific Guidelines

### Controllers (Employee Project)
- Keep controllers thin - delegate business logic to services
- Use proper HTTP status codes
- Implement model validation
- Handle exceptions appropriately
- Use dependency injection for services

```csharp
[HttpPost]
public async Task<IActionResult> Create(EmployeeViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);
    
    try
    {
        var result = await _employeeService.SaveEmployee(model);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        // Log error and return appropriate response
        return View("Error");
    }
}
```

### Services (Business Logic Layer)
- Implement business rules and validation
- Use try-catch blocks for error handling
- Return consistent response objects
- Use AutoMapper for object mapping
- Keep methods focused and single-purpose

```csharp
public async Task<Responses<long>> SaveEmployee(EmployeeViewModel model)
{
    try
    {
        // Validate business rules
        if (await IsEmailExist(model.Email, model.Id))
            return new Responses<long>(data: 0, statusCode: HttpStatusCode.BadRequest, 
                                     message: "Email already exists");
        
        var employee = _mapper.Map<Employee>(model);
        var result = await _employeeRepository.SaveEmployee(employee);
        
        return new Responses<long>(data: result, statusCode: HttpStatusCode.OK, 
                                 message: "Employee saved successfully");
    }
    catch (Exception ex)
    {
        // Log exception
        return new Responses<long>(data: 0, statusCode: HttpStatusCode.InternalServerError, 
                                 message: "An error occurred while saving employee");
    }
}
```

### Repositories (Data Access Layer)
- Use Entity Framework Core best practices
- Implement proper async/await patterns
- Use parameterized queries to prevent SQL injection
- Handle database exceptions
- Follow repository pattern conventions

```csharp
public async Task<Employee> GetEmployee(long id)
{
    try
    {
        using var db = new EmployeeDbContext();
        return await db.Employees
            .Where(a => a.Id == id && a.IsDeleted == false)
            .FirstOrDefaultAsync();
    }
    catch (Exception ex)
    {
        // Log exception and rethrow or handle appropriately
        throw;
    }
}
```

### Shared Models & DTOs
- Use data annotations for validation
- Keep models focused and cohesive
- Implement proper property naming
- Use appropriate data types

```csharp
public class EmployeeViewModel
{
    public long Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; set; }
}
```

---

## Example

```csharp
// Good example following the guidelines
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        this.employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> GetAllEmployees(EmployeeRequestModel model)
    {
        // Handle possible errors from repository/API
        try
        {
            var result = await employeeRepository.GetAllEmployees(model);
            return _mapper.Map<List<EmployeeDto>>(result);
        }
        catch (Exception ex)
        {
            // Log and handle error appropriately
            throw new ApplicationException("Failed to get employees", ex);
        }
    }
}
```

---

## Git Workflow and Pull Request Process

### Branch Management
- **Create feature branches** from `dev` for new features or bug fixes
- **Use descriptive branch names**: `feature/employee-validation`, `bugfix/date-parsing`, `refactor/service-cleanup`
- **Keep branches focused** on a single feature or fix

### Automated Git Diff Review Process

#### **Using Custom Code Reviewer Agent**
The project includes a specialized Git Diff Code Review Assistant that can automatically run git diff commands and provide comprehensive analysis:

```bash
# The reviewer agent can execute these commands automatically:
git status                    # Check file status
git diff                      # View all unstaged changes
git diff --staged             # View staged changes
git diff <filepath>           # View specific file changes
git diff main..dev            # Compare branches
git diff --stat               # View change statistics
git add <filepath>            # Stage specific files
git add .                     # Stage all changes
git commit -m "message"       # Commit with message
git push origin <branch>      # Push to remote
gh pr create                  # Create pull request
```

#### **Automated Review Capabilities**
- **Security Analysis**: OWASP compliance checking on changed code
- **Standards Validation**: Project coding standards enforcement
- **Architecture Review**: Clean architecture principle adherence
- **Performance Assessment**: Code efficiency evaluation
- **Git Workflow Validation**: Proper staging and commit practices
- **Automated Staging**: Stage approved changes automatically
- **Commit Generation**: Create descriptive commit messages
- **Push Operations**: Push changes to remote repositories
- **PR Creation**: Generate and create pull requests
- **Workflow Automation**: Complete git workflows from review to PR

### Staging and Committing Changes

1. **Check status before staging**:
   ```bash
   git status
   ```

2. **Review changes before staging** (Can be automated via Code Reviewer Agent):
   ```bash
   # View all unstaged changes
   git diff
   
   # View changes in a specific file
   git diff path/to/specific/file.cs
   
   # View staged changes (after git add)
   git diff --staged
   
   # View changes between branches
   git diff main..dev
   ```

3. **Stage specific files** (recommended):
   ```bash
   git add path/to/specific/file.cs
   ```
   Or stage all changes:
   ```bash
   git add .
   ```

4. **Write meaningful commit messages**:
   ```bash
   git commit -m "Add error handling to EmployeeService methods"
   git commit -m "Fix date parsing issue in SaveEmployee method"
   git commit -m "Refactor: Remove commented code and unused variables"
   ```

5. **Push changes to remote branch**:
   ```bash
   git push origin feature/your-branch-name
   ```

### Creating Pull Requests

1. **Navigate to repository** on GitHub: https://github.com/durgeshpiogc321/Employee
2. **Click "Compare & pull request"** for your branch
3. **Fill in PR details**:
   - **Title**: Clear, descriptive summary of changes
   - **Description**: Explain what was changed and why
   - **Link issues**: Reference related issues if applicable
4. **Request reviewers** and assign appropriate labels
5. **Submit the pull request**

### Code Review Guidelines

#### For Code Authors:
- **Self-review** your code before submitting PR
- **Ensure all tests pass** before requesting review
- **Keep PRs small and focused** (ideally < 400 lines of changes)
- **Respond promptly** to review feedback

#### For Code Reviewers:
- **Review within 24-48 hours** when possible
- **Check for**:
  - Code follows project conventions
  - Proper error handling is implemented
  - No commented-out code or unused variables
  - Security and performance considerations
  - Tests are included for new functionality

#### Review Checklist:
- [ ] **Code Style**: Follows naming conventions and formatting
- [ ] **Error Handling**: Appropriate try-catch blocks and meaningful error messages
- [ ] **Performance**: No obvious performance issues
- [ ] **Security**: No sensitive data exposed or security vulnerabilities
- [ ] **Tests**: Unit tests included for new features
- [ ] **Documentation**: Code is properly commented
- [ ] **Functionality**: Code does what it's supposed to do

### Pull Request Template

```markdown
## Summary
Brief description of changes made.

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Refactoring
- [ ] Documentation update

## Testing
- [ ] Unit tests pass
- [ ] Manual testing completed
- [ ] No regression issues

## Checklist
- [ ] Code follows project style guidelines
- [ ] Self-review completed
- [ ] Comments added for complex logic
- [ ] No commented-out code or debug statements
```

### Merging Guidelines

1. **Require at least one approval** before merging
2. **Use "Squash and merge"** for feature branches to keep history clean
3. **Delete feature branches** after successful merge
4. **Update local dev branch** after merge:
   ```bash
   git checkout dev
   git pull origin dev
   ```

---

## Automated Tools Integration

- **Use GitHub Actions** for CI/CD pipeline
- **Enable branch protection rules** for `main` and `dev` branches
- **Require status checks** before merging
- **Use code analysis tools** like CodeQL for security scanning

---

Please follow these guidelines for all code contributions and Git workflow processes.