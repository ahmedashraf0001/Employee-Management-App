using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ITI_Project.Database;
using ITI_Project.UI;

namespace ITI_Project.Services
{
    internal class DepartmentServices
    {
        #region Variables
        private readonly CompanyDbContext context;
        #endregion
       
        #region Ctor
        public DepartmentServices(CompanyDbContext cntx)
        {
            context = cntx;
        }
        #endregion
        
        #region Services
        public void NewDepartment()
        {
            #region variables
            string? _name;
            int _id = 0;
            HashSet<int> ids = new HashSet<int>(context.Departments.Select(x => x.Id));
            HashSet<string> names = new HashSet<string>(context.Departments.Select(x => x.Name));
            #endregion

            #region input
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Add New Department **********\n");
            Console.ResetColor();

            Console.Write("Enter Department ID: ");
            while (!int.TryParse(Console.ReadLine(), out _id) || ids.Contains(_id))
            {
                Console.WriteLine();
                Console.Write("Invalid or ID already exists, try again: ");
            }
            Console.WriteLine();

            Console.Write("Enter Department Name: ");
            _name = Console.ReadLine();
            while (string.IsNullOrEmpty(_name) || names.Contains(_name))
            {
                Console.WriteLine();
                Console.Write("Invalid or Name already exists, try again: ");
                _name = Console.ReadLine();
            }
            #endregion

            #region Querying
            Department dept = new Department { Id = _id, Name = _name };
            context.Departments.Add(dept);
            context.SaveChanges();
            #endregion

            #region Output
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Department Added Successfully **********\n");
            Console.ResetColor();

            Console.WriteLine("+----------------------+----------------------------+");
            Console.WriteLine("|   Department ID      |      Department Name       |");
            Console.WriteLine("+----------------------+----------------------------+");
            Console.WriteLine($"| {dept.Id,-20} | {dept.Name,-26} |");
            Console.WriteLine("+----------------------+----------------------------+");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            #endregion
        }
        public void EditDepartment()
        {
            #region input
            int _id;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search by Department ID **********\n");
            Console.ResetColor();
            if (!context.Departments.Any())
            {
                Console.Write("No Departments in The Database. ");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Department's ID: ");
            while (!int.TryParse(Console.ReadLine(), out _id) || !context.Departments.Any(d => d.Id == _id))
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

            var department = context.Departments
                                    .Where(x => x.Id == _id)
                                    .ToList();

            Console.WriteLine("+----------------------+----------------------------+");
            Console.WriteLine("|   Department ID      |      Department Name       |");
            Console.WriteLine("+----------------------+----------------------------+");

            if (!department.Any())
            {
                Console.WriteLine("|                  No results found.                |");
                Console.WriteLine("+----------------------+----------------------------+");
                Console.WriteLine("Press any key to Exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                foreach (var dept in department)
                {
                    Console.WriteLine($"| {dept.Id,-20} | {dept.Name,-26} |");
                    Console.WriteLine("+----------------------+----------------------------+");
                }
            }
            Console.WriteLine("Press any key to continue editing or ESC to Exit...");
            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                return;
            }

            string[] innerMenu = { " ID ", " Name ", " Exit " };
            Action[] actions = {
        () => {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Edit ID **********\n");
            Console.ResetColor();
            Console.Write("Enter New Department ID: ");
            int newId;
            while (!int.TryParse(Console.ReadLine(), out newId) || newId == _id || context.Departments.Any(d => d.Id == newId))
            {
                Console.Write("Invalid ID or ID already exists, Try again: ");
            }

            var oldDepartment = context.Departments.First(d => d.Id == _id);
            var newDepartment = new Department
            {
                Id = newId,
                Name = oldDepartment.Name
            };

            context.Departments.Add(newDepartment);
            context.Departments.Remove(oldDepartment);

            var employeesToUpdate = context.Employees.Where(e => e.DepartmentId == _id).ToList();
            foreach (var employee in employeesToUpdate)
            {
                employee.DepartmentId = newId;
            }

            context.SaveChanges();
            _id = newId;
            ShowSuccessMessage("Department ID Edited successfully!", "Edited");
        },

        () => {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Edit Name **********\n");
            Console.ResetColor();
            Console.Write("Enter New Department Name: ");
            string newName;
            newName = Console.ReadLine();
            while (string.IsNullOrEmpty(newName))
            {
                Console.Write("Empty Name Field, Try again: ");
                newName = Console.ReadLine();
            }

            var departmentToUpdate = context.Departments.First(d => d.Id == _id);
            departmentToUpdate.Name = newName;

            context.SaveChanges();

            ShowSuccessMessage("Department Name Edited successfully!", "Edited");
        },

        () => { }
    };

            EmployeeUI.MenuDisplay(" Choose what you want to Edit : ", innerMenu, actions);
            #endregion
        }
        public void DeleteDepartment()
        {
            #region input
            int _id;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Search by Department ID **********\n");
            Console.ResetColor();
            if (!context.Departments.Any())
            {
                Console.Write("No Departments in The Database. ");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Department's ID: ");
            while (!int.TryParse(Console.ReadLine(), out _id) || !context.Departments.Any(d => d.Id == _id))
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

            var department = context.Departments
                                    .Where(x => x.Id == _id)
                                    .ToList();

            Console.WriteLine("+----------------------+----------------------------+");
            Console.WriteLine("|   Department ID      |      Department Name       |");
            Console.WriteLine("+----------------------+----------------------------+");

            if (!department.Any())
            {
                Console.WriteLine("|                  No results found.                |");
                Console.WriteLine("+----------------------+----------------------------+");
                Console.WriteLine("Press any key to Exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                foreach (var dept in department)
                {
                    Console.WriteLine($"| {dept.Id,-20} | {dept.Name,-26} |");
                    Console.WriteLine("+----------------------+----------------------------+");
                }
            }
            Console.WriteLine("Press any key to continue Deleting or ESC to Exit...");
            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                return;
            }

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Delete Department **********\n");
            Console.ResetColor();

            var oldDepartment = context.Departments.First(d => d.Id == _id);

            context.Departments.Remove(oldDepartment);

            var employeesToUpdate = context.Employees.Where(e => e.DepartmentId == _id).ToList();
            foreach (var employee in employeesToUpdate)
            {
                employee.DepartmentId = null;
            }

            context.SaveChanges();
            ShowSuccessMessage("Department ID Deleted Successfully!", "Deleted");

            #endregion
        }
        public void ClearDepartments()
        {
            var oldDepartments = context.Departments.Select(x =>x);
            foreach (var department in oldDepartments)
                { context.Departments.Remove(department); }

            var employeesToUpdate = context.Employees.Select(X=>X);
            foreach (var employee in employeesToUpdate)
            {
                employee.DepartmentId = null;
            }
            context.SaveChanges();
            Console.WriteLine("  - Department Deleted.");
        }
        public static void ShowSuccessMessage(string message, string header)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"********** {header} Successfully **********\n");
            Console.ResetColor();
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        public void DisplayDepartments()
        {
            #region variables
            var Depts = context.Departments.Select(x => x);
            #endregion

            #region Output
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Department List **********\n");
            Console.ResetColor();

            Console.WriteLine("+----------------------+----------------------------+");
            Console.WriteLine("|   Department ID      |      Department Name       |");
            Console.WriteLine("+----------------------+----------------------------+");
            if (!Depts.Any())
            {
                Console.WriteLine("|                  No results found.                |");
                Console.WriteLine("+----------------------+----------------------------+");
                Console.WriteLine("Press any key to Exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                foreach (var dept in Depts)
                {
                    Console.WriteLine($"| {dept.Id,-20} | {dept.Name,-26} |");
                    Console.WriteLine("+----------------------+----------------------------+");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            #endregion
        }
        #endregion
    }
}
