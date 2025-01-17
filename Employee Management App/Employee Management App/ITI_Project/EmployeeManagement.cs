using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI_Project.Database;
using ITI_Project.Services;
using ITI_Project.UI;

namespace ITI_Project
{
    public class EmployeeManagement
    {
        #region Variables
        DepartmentServices Deptservices;
        EmployeeServices EmpServices;
        private readonly CompanyDbContext context;
        #endregion
        
        #region Ctor
        public EmployeeManagement()
        {
            context = new CompanyDbContext();
            Deptservices = new DepartmentServices(context);
            EmpServices = new EmployeeServices(context);
        }
        #endregion

        #region Run
        public void Run()
        {
            Action[] actions = { New, Edit, Delete, Search, Display, () => { } };
            EmployeeUI.MainMenu(actions);
        }
        #endregion

        #region Main Functions
        public void New()
        {
            string[] innerMenu = { " New Department ", " New Employee ", " Exit " };
            Action[] actions = { Deptservices.NewDepartment, EmpServices.NewEmployee, () => { } };
            EmployeeUI.MenuDisplay(" Choose what you want to Add : ", innerMenu, actions);
        }
        public void Edit()
        {
            string[] innerMenu = { " Edit Department ", " Edit Employee ", " Exit " };
            Action[] actions = { Deptservices.EditDepartment, EmpServices.EditEmployee, () => { } };
            EmployeeUI.MenuDisplay(" Choose what you want to Edit : ", innerMenu, actions);
        }
        public void Delete()
        {
            string[] innerMenu = { " Delete Department ", " Delete Employee " ," Clear Database (Caution) ", " Exit " };
            Action[] actions = { Deptservices.DeleteDepartment, EmpServices.DeleteEmployee, DatabaseClear, ()=> { } };
            EmployeeUI.MenuDisplay(" Choose what you want to Delete : ", innerMenu, actions);
        }
        public void Search()
        {
            string[] innerMenu = { " ID ", " Name ", " Age ", " Exit " };
            Action[] actions = { EmpServices.SearchById, EmpServices.SearchByName, EmpServices.SearchByAge, () => { } };
            EmployeeUI.MenuDisplay(" Choose how you want to Search : ", innerMenu, actions);
        }
        public void Display()
        {
            string[] innerMenu = { " Display Department ", " Display Employees ", " Exit " };
            Action[] actions = { Deptservices.DisplayDepartments, EmpServices.DisplayEmployees, () => { } };
            EmployeeUI.MenuDisplay(" Choose what you want to Display : ", innerMenu, actions);
        }
        public void DatabaseClear()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("********** Database Deletion **********\n");
            Console.ResetColor();

            var employees = context.Employees.Select(x => x);
            var Depts = context.Departments.Select(x => x);

            Console.WriteLine("+----------------------+----------------------------+");
            Console.WriteLine("|   Department ID      |      Department Name       |");
            Console.WriteLine("+----------------------+----------------------------+");
            foreach (var dept in Depts)
            {
                Console.WriteLine($"| {dept.Id,-20} | {dept.Name,-26} |");
                Console.WriteLine("+----------------------+----------------------------+");
            }
            Console.WriteLine();
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            Console.WriteLine("| Employee ID |       Employee Name       |  Age  |     Salary      |     Department    |");
            Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");

            foreach (var employee in employees)
            {
                string deptName = employee.Department?.Name ?? "N/A";
                Console.WriteLine($"| {employee.Id,-12}| {employee.Name,-25} | {employee.Age,-5} | {employee.Salary,-15:C} | {deptName,-17} |");
                Console.WriteLine("+-------------+---------------------------+-------+-----------------+-------------------+");
            }
            Console.BackgroundColor= ConsoleColor.DarkRed;    
            Console.WriteLine("\nNote: You Cannot Restore The Database After.");
            Console.ResetColor();
            Console.WriteLine("\nAre you sure you want to perform this action?");
            Console.Write("[Y] Yes  [N] No : ");

            string input = Console.ReadLine();
            input= input.ToLower();
            switch (input)
            {
                case "y":
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("********** Database Deletion **********\n");
                    Console.ResetColor();
                    Console.WriteLine("Performing Deletion...");
                    EmpServices.ClearEmployees();
                    Deptservices.ClearDepartments();
                    Console.Write("\nDatabase Cleared Sucessfully!");
                    Console.ReadKey();
                    break;
                case "n":
                    return;
            }
        }
        #endregion
    }
}
