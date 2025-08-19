# Copilot Coding Instructions

This document outlines the coding standards and best practices for the Employee project. All contributors and AI code generation tools (such as GitHub Copilot) should follow these guidelines to ensure code consistency, readability, and maintainability.

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

### Staging and Committing Changes

1. **Check status before staging**:
   ```bash
   git status
   ```

2. **Stage specific files** (recommended):
   ```bash
   git add path/to/specific/file.cs
   ```
   Or stage all changes:
   ```bash
   git add .
   ```

3. **Write meaningful commit messages**:
   ```bash
   git commit -m "Add error handling to EmployeeService methods"
   git commit -m "Fix date parsing issue in SaveEmployee method"
   git commit -m "Refactor: Remove commented code and unused variables"
   ```

4. **Push changes to remote branch**:
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