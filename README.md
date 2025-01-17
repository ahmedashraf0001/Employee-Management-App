# Employee Management Console App

This is a simple console application for managing employees and departments using Entity Framework (EF). It provides an intuitive interface to store, retrieve, update, and delete employee and department data efficiently.

## Features

The application offers the following core functionalities:

1. **Add New Entries**  
   - Add a new employee: Enter employee ID, name, age, and salary. A table of available departments is displayed for selection.  
   - Add a new department: Provide department ID and name.

2. **Edit Existing Entries**  
   - Edit employee details, including their associated department.  
   - Modify department information.

3. **Delete Entries**  
   - Remove an employee from the system.  
   - Delete a department (with appropriate validations, if necessary).

4. **Search for Employees**  
   - Search employees by ID, name, or age.

5. **Display Data**  
   - View all employees in a tabular format.  
   - Display all departments in a tabular format.

## Requirements

- **.NET Framework or .NET Core**  
- **Entity Framework (EF)** for ORM functionality  
- A database compatible with EF (e.g., SQL Server, SQLite)

## Getting Started

### Restore dependencies and build the project:

```bash
dotnet restore
dotnet build
```
### Run the application:

```bash
dotnet run
```
### Usage Instructions:

 - New: Add a new employee or department.
 - Edit: Modify existing employee or department records.
 - Delete: Remove an employee or department.
 - Search: Look up employees by ID, name, or age.
 - Display: Show all employees or departments in a tabular format.
