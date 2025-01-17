using ITI_Project.Database;
using ITI_Project.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_Project.Services
{
    internal class EmployeeServices
    {
        #region Variables
        private readonly CompanyDbContext context;
        #endregion

        #region Ctor
        public EmployeeServices(CompanyDbContext cntx)
        {
            context = cntx;
        }
        #endregion
      
        #region Services
        public void NewEmployee()
        {
            #region variables
            string _Name;
            int _Age;
            double _Salary;
            int _DepartmentId;
            HashSet<int> _DeptIds = new HashSet<int>(context.Departments.Select(x => x.Id));
            #endregion

            #region input
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Add New Employee **********\n");
            Console.ResetColor();

            Console.Write(" Enter Name: ");
            _Name = Console.ReadLine();
            while (string.IsNullOrEmpty(_Name))
            {
                Console.Write(" Name cannot be empty. Try again: ");
                _Name = Console.ReadLine();
            }
            Console.WriteLine();

            Console.Write(" Enter Age: ");
            while (!int.TryParse(Console.ReadLine(), out _Age) || _Age < 0 || _Age > 120)
            {
                Console.WriteLine();
                Console.Write(" Invalid age. Please enter a valid age between 0 and 120: ");
            }
            Console.WriteLine();

            Console.Write(" Enter Department ID ");
            Console.BackgroundColor= ConsoleColor.DarkBlue;
            Console.Write(" [Type {None} To Skip / Type {Show} To Display Available Departments]:");
            Console.ResetColor();
            string check = Console.ReadLine();
            var Depts = context.Departments.Select(x => x);

            if (string.Equals(check, "None", StringComparison.OrdinalIgnoreCase) || string.Equals(check, "none", StringComparison.OrdinalIgnoreCase))
            {
                _DepartmentId = -1;
            }
            else if (string.Equals(check, "Show", StringComparison.OrdinalIgnoreCase) || string.Equals(check, "Show", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine(" +----------------------+----------------------------+");
                Console.WriteLine(" |   Department ID      |      Department Name       |");
                Console.WriteLine(" +----------------------+----------------------------+");

                if (!Depts.Any())
                {
                    Console.WriteLine(" |                  No results found.                |");
                    Console.WriteLine(" +----------------------+----------------------------+");
                    Console.WriteLine("Press any key to Exit...");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    foreach (var dept in Depts)
                    {
                        Console.WriteLine($" | {dept.Id,-20} | {dept.Name,-26} |");
                        Console.WriteLine(" +----------------------+----------------------------+");
                    }
                }
                Console.Write(" Enter Department ID: ");
                check = Console.ReadLine();
                while (!int.TryParse(check, out _DepartmentId) || !_DeptIds.Contains(_DepartmentId))
                {
                    Console.WriteLine();
                    Console.Write(" Invalid Department ID or it does not exist. Type {None} to skip or try again: ");
                    check = Console.ReadLine();
                    if (string.Equals(check, "None", StringComparison.OrdinalIgnoreCase) || string.Equals(check, "none", StringComparison.OrdinalIgnoreCase))
                    {
                        _DepartmentId = -1;
                        break;
                    }
                }
            }
            else
            {
                while (true)
                {
                    if (int.TryParse(check, out _DepartmentId) && _DeptIds.Contains(_DepartmentId))
                    {
                        break;
                    }
                    Console.WriteLine();
                    Console.Write(" Invalid Department ID or it does not exist. Type {None} to skip or try again: ");
                    check = Console.ReadLine();


                    if (string.Equals(check, "None", StringComparison.OrdinalIgnoreCase) || string.Equals(check, "none", StringComparison.OrdinalIgnoreCase))
                    {
                        _DepartmentId = -1;
                        break;
                    }
                    else if (string.Equals(check, "Show", StringComparison.OrdinalIgnoreCase) || string.Equals(check, "Show", StringComparison.OrdinalIgnoreCase))
                    {

                        Console.WriteLine(" +----------------------+----------------------------+");
                        Console.WriteLine(" |   Department ID      |      Department Name       |");
                        Console.WriteLine(" +----------------------+----------------------------+");

                        if (!Depts.Any())
                        {
                            Console.WriteLine(" |                  No results found.                |");
                            Console.WriteLine(" +----------------------+----------------------------+");
                            Console.WriteLine("Press any key to Exit...");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            foreach (var dept in Depts)
                            {
                                Console.WriteLine($" | {dept.Id,-20} | {dept.Name,-26} |");
                                Console.WriteLine(" +----------------------+----------------------------+");
                            }
                        }

                        Console.Write(" Enter Department ID : ");
                        check = Console.ReadLine();
                        if (int.TryParse(check, out _DepartmentId) && _DeptIds.Contains(_DepartmentId))
                        {
                            break;
                        }
                    }
                }
            }
            Console.WriteLine();

            Console.Write(" Enter Salary: ");
            while (!double.TryParse(Console.ReadLine(), out _Salary) || _Salary < 0)
            {
                Console.WriteLine();
                Console.Write(" Invalid salary. Please enter a valid salary amount: ");
            }
            Console.WriteLine();
            #endregion

            #region Querying

            Employee employee = new Employee
            {
                Name = _Name,
                Age = _Age,
                Salary = _Salary,
                DepartmentId = _DepartmentId==-1?null: _DepartmentId
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Department Added Successfully **********\n");
            Console.ResetColor();
            Console.WriteLine("\nEmployee added successfully!\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            #endregion
        }
        public void EditEmployee()
        {
            #region input
            int _id;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search by Employee ID **********\n");
            Console.ResetColor();
            if (!context.Employees.Any())
            {
                Console.Write("No Employees In The Database.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Employee's ID: ");
            while (!int.TryParse(Console.ReadLine(), out _id))
            {
                Console.Write("Invalid ID or does not exist, try again: ");
            }
            #endregion

            #region output
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search Results **********\n");
            Console.ResetColor();

            var employee = context.Employees
                                   .Where(x => x.Id == _id)
                                   .FirstOrDefault();


            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            Console.WriteLine("| Employee ID |       Employee Name       |  Age  |     Salary      |     Department    |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            if (employee == null)
            {
                Console.WriteLine("|                                   No results found.                                   |");
                Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+\n");
                Console.WriteLine("Press any key to Exit...");
                Console.ReadKey();
                return;
            }

            string deptName = employee.Department?.Name ?? "N/A";
            Console.WriteLine($"| {employee.Id,-12}| {employee.Name,-25} | {employee.Age,-5} | {employee.Salary,-15:C} | {deptName,-17} |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+\n");

            Console.WriteLine("Press any key to continue editing or esc to Exit...");
            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                return;
            }
            string[] innerMenu = { " Name ", " Age ", " Salary ", " Department ", " Exit " };
            Action[] actions = {
                () => { Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("********** Edit Name **********\n");
                        Console.ResetColor();
                        Console.Write("Enter Name: ");
                        string _name = Console.ReadLine();
                        employee.Name =_name;
                        context.SaveChanges();
                        DepartmentServices.ShowSuccessMessage("Employee Name Edited successfully!", "Edited");
                       },

                () => { Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("********** Edit Age **********\n");
                        Console.ResetColor();
                        Console.Write("Enter Age: ");
                        int _Age;
                        while (!int.TryParse(Console.ReadLine(), out _Age) || _Age < 0 || _Age > 120)
                        {
                            Console.Write(" Invalid Age. Please enter a valid age between 0 and 120: ");
                        }
                        employee.Age =_Age;
                        context.SaveChanges();
                        DepartmentServices.ShowSuccessMessage("Employee Age Edited successfully!", "Edited");
                      },

                () => { Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("********** Edit Salary **********\n");
                        Console.ResetColor();
                        Console.Write("Enter Salary: ");
                        int _salary;
                        while (!int.TryParse(Console.ReadLine(), out _salary) || _salary < 0 )
                        {
                            Console.Write(" Invalid Salary. Please enter a valid Salary: ");
                        }
                        employee.Salary =_salary;
                        context.SaveChanges();
                        DepartmentServices.ShowSuccessMessage("Employee Salary Edited successfully!", "Edited");
                      },
                
                () => {
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("********** Edit Department **********\n");
                        Console.ResetColor();
                        Console.WriteLine($"Current Department: {employee.Department?.Name ?? "N/A"}\n");

                        var departments = context.Departments.ToList();
                        Console.WriteLine("Available Departments:");
                        Console.WriteLine("+----------------------+----------------------------+");
                        Console.WriteLine("|   Department ID      |      Department Name       |");
                        Console.WriteLine("+----------------------+----------------------------+");

                        foreach (var dept in departments)
                        {
                            Console.WriteLine($"| {dept.Id,-20} | {dept.Name,-26} |");
                                    Console.WriteLine("+----------------------+----------------------------+");
                        }

                        Console.Write("Enter the ID of the new department: ");
                        int departmentId;
                        while (!int.TryParse(Console.ReadLine(), out departmentId) || !departments.Any(d => d.Id == departmentId))
                        {
                            Console.Write("Invalid ID. Please enter a valid department ID: ");
                        }
                        var newdept = context.Employees.FirstOrDefault(x => x.Id == _id);
                        newdept.DepartmentId = departmentId;
                        context.SaveChanges();
                        DepartmentServices.ShowSuccessMessage($"Employee Department changed to '{newdept.Department.Name}' successfully!", "Edited");

                      },

                () => { } };

            EmployeeUI.MenuDisplay(" Choose what you want to Edit : ", innerMenu, actions);
            #endregion
        }
        public void ClearEmployees()
        {
            var oldEmployees = context.Employees.Select(X => X);
            foreach (var employee in oldEmployees)
            {
                context.Employees.Remove(employee);
            }
            context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Employees';");
            context.SaveChanges();
            Console.WriteLine("\n  - Employees Deleted.");
        }
        public void DeleteEmployee()
        {
            #region input
            int _id;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search by Employee ID **********\n");
            Console.ResetColor();
            if (!context.Employees.Any())
            {
                Console.Write("No Employees in The Database. ");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter Employee's ID: ");
            while (!int.TryParse(Console.ReadLine(), out _id))
            {
                Console.Write("Invalid ID or does not exist, try again: ");
            }
            #endregion

            #region output
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search Results **********\n");
            Console.ResetColor();

            var employee = context.Employees
                                   .Where(x => x.Id == _id)
                                   .FirstOrDefault();


            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            Console.WriteLine("| Employee ID |       Employee Name       |  Age  |     Salary      |     Department    |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            if (employee == null)
            {
                Console.WriteLine("|                                   No results found.                                   |");
                Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+\n");
                Console.WriteLine("Press any key to Exit...");
                Console.ReadKey();
                return;
            }

            string deptName = employee.Department?.Name ?? "N/A";
            Console.WriteLine($"| {employee.Id,-12}| {employee.Name,-25} | {employee.Age,-5} | {employee.Salary,-15:C} | {deptName,-17} |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+\n");

            Console.WriteLine("Press any key to continue deleting or esc to Exit...");
            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                return;
            }
            var oldEmployee = context.Employees.First(d => d.Id == _id);

            context.Employees.Remove(oldEmployee);

            context.SaveChanges();
            DepartmentServices.ShowSuccessMessage("Employee ID Deleted Successfully!", "Deleted");
            #endregion
        }
        public void SearchById()
        {
            #region input
            int _id;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search by Employee ID **********\n");
            Console.ResetColor();
            if (!context.Employees.Any())
            {
                Console.Write(" No Employees in The Database. ");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter Employee's ID: ");
            while (!int.TryParse(Console.ReadLine(), out _id))
            {
                Console.Write("Invalid ID, try again: ");
            }
            #endregion

            #region output
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search Results **********\n");
            Console.ResetColor();

            var employee = context.Employees
                                   .Where(x => x.Id == _id)
                                   .ToList();


            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            Console.WriteLine("| Employee ID |       Employee Name       |  Age  |     Salary      |     Department    |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            if (!employee.Any())
            {
                Console.WriteLine("|                                   No results found.                                   |");
                Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            }
            else
            {
                foreach (var emp in employee)
                {
                    string deptName = emp.Department?.Name ?? "N/A";
                    Console.WriteLine($"| {emp.Id,-12}| {emp.Name,-25} | {emp.Age,-5} | {emp.Salary,-15:C} | {deptName,-17} |");
                    Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            #endregion
        }
        public void SearchByName()
        {
            #region input
            string _name;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search by Employee Name **********\n");
            Console.ResetColor();
            if (!context.Employees.Any())
            {
                Console.Write(" No Employees in The Database. ");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter Employee's Name: ");
            _name = Console.ReadLine();

            // Validate input for empty names
            while (string.IsNullOrWhiteSpace(_name))
            {
                Console.Write("Invalid name. Try again: ");
                _name = Console.ReadLine();
            }
            #endregion

            #region output
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search Results **********\n");
            Console.ResetColor();

            var employees = context.Employees
                                   .Where(x => x.Name.Contains(_name))
                                   .ToList();

            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            Console.WriteLine("| Employee ID |       Employee Name       |  Age  |     Salary      |     Department    |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            if (!employees.Any())
            {
                Console.WriteLine("|                                   No results found.                                   |");
                Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            }
            else
            {
                foreach (var employee in employees)
                {
                    string deptName = employee.Department?.Name ?? "N/A";
                    Console.WriteLine($"| {employee.Id,-12}| {employee.Name,-25} | {employee.Age,-5} | {employee.Salary,-15:C} | {deptName,-17} |");
                    Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            #endregion
        }
        public void SearchByAge()
        {
            #region input
            int _Age;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search by Employee Age **********\n");
            Console.ResetColor();
            if (!context.Employees.Any())
            {
                Console.Write(" No Employees in The Database. ");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter Employee's Age: ");
            while (!int.TryParse(Console.ReadLine(), out _Age))
            {
                Console.Write("Invalid Age or does not exist, try again: ");
            }
            #endregion

            #region output
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search Results **********\n");
            Console.ResetColor();

            var employee = context.Employees
                                   .Where(x => x.Age == _Age)
                                   .ToList();


            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            Console.WriteLine("| Employee ID |       Employee Name       |  Age  |     Salary      |     Department    |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            if (!employee.Any())
            {
                Console.WriteLine("|                                   No results found.                                   |");
                Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");

            }
            else
            {
                foreach (var emp in employee)
                {
                    string deptName = emp.Department?.Name ?? "N/A";
                    Console.WriteLine($"| {emp.Id,-12}| {emp.Name,-25} | {emp.Age,-5} | {emp.Salary,-15:C} | {deptName,-17} |");
                    Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            #endregion
        }
        public void DisplayEmployees()
        {
            #region variables
            var employees = context.Employees.Select(x => x);
            #endregion

            #region Output
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Employee List **********\n");
            Console.ResetColor();

            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            Console.WriteLine("| Employee ID |       Employee Name       |  Age  |     Salary      |     Department    |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");

            if (!employees.Any())
            {
                Console.WriteLine("|                                   No results found.                                   |");
                Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            }
            else
            {
                foreach (var emp in employees)
                {
                    string deptName = emp.Department?.Name ?? "N/A";
                    Console.WriteLine($"| {emp.Id,-12}| {emp.Name,-25} | {emp.Age,-5} | {emp.Salary,-15:C} | {deptName,-17} |");
                    Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            #endregion
        }
        #endregion
    }
}
