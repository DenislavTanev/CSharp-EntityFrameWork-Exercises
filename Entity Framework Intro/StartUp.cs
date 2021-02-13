using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            string result = RemoveTown(context);

            Console.WriteLine(result);
        }

        //Problem 03.
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .OrderBy(e => e.EmployeeId)
                .ToList();

            foreach (Employee e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 04.
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToList();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 05.
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(s => s.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .ToList();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 06.

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            Employee empl = context
                .Employees
                .First(e => e.LastName == "Nakov");

            context.Addresses.Add(newAddress);
            empl.Address = newAddress;

            context.SaveChanges();

            var addresses = context
                 .Employees
                 .OrderByDescending(e => e.AddressId)
                 .Take(10)
                 .Select(e => e.Address.AddressText)
                 .ToList();

            foreach (var address in addresses)
            {
                sb.AppendLine(address);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 07.
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 &&
                                                          ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        StartDate = ep.Project.StartDate
                        .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = ep.Project.EndDate.HasValue
                        ? ep.Project
                            .EndDate
                            .Value
                            .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                            : "not finished"
                    })
                    .ToList()
                })
                .ToList();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 08.
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresess = context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmplCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmplCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            foreach (var a in addresess)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmplCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 09.
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
               .Where(e => e.EmployeeId == 147)
               .Select(e => new
               {
                   e.FirstName,
                   e.LastName,
                   e.JobTitle,
                   Projects = e.EmployeesProjects
                   .Select(ep => new
                   {
                       ProjectName = ep.Project.Name,
                   })
                   .OrderBy(p => p.ProjectName)
                   .ToList()
               })

               .ToList();

            foreach (var empl in employee)
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle}");

                foreach (var project in empl.Projects)
                {
                    sb.AppendLine($"{project.ProjectName}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 10.
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
               .Where(d => d.Employees.Count > 5)
               .Select(d => new
               {
                   d.Name,
                   ManagerFirstName = d.Manager.FirstName,
                   ManagerLastName = d.Manager.LastName,
                   DepEmployee = d.Employees
                   .Select(e => new
                   {
                       EmployeeFirstName = e.FirstName,
                       EmployeeLastName = e.LastName,
                       e.JobTitle
                   })
                   .OrderBy(e => e.EmployeeFirstName)
                   .ThenBy(e => e.EmployeeLastName)
                   .ToList()
               })
               .ToList();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} – {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var e in d.DepEmployee)
                {
                    sb.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 11.
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderByDescending(p => p.StartDate.Date)
                .Take(10)
                .OrderByDescending((p => p.Name.Length))
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}")
                    .AppendLine($"{p.Description}")
                    .AppendLine($"{p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12.
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();



            IQueryable<Employee> empl = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services");

            foreach (var e in empl)
            {
                e.Salary += e.Salary * 0.12m;
            }

            context.SaveChanges();

            var employee = empl
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 13.
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var empl = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary,
                    e.JobTitle
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var e in empl)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 14.
        public static string DeleteProjectById(SoftUniContext context)
        {

            StringBuilder sb = new StringBuilder();

            var project = context.Projects.Find(2);


            IQueryable<EmployeeProject> empl = context.EmployeesProjects
                .Where(p => p.ProjectId == project.ProjectId);

            foreach (var ep in empl)
            {
                context.EmployeesProjects.Remove(ep);
            }

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context
                .Projects
                .Take(10)
                .Select(p => new
                {
                    p.Name
                })
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 15.
        public static string RemoveTown(SoftUniContext context)
        {
            Town town = context.Towns.First(t => t.Name == "Seattle");

            IQueryable<Address> addresses = context.Addresses.Where(a => a.TownId == town.TownId);

            int count = addresses.Count();

            IQueryable<Employee> empl = context.Employees
                .Where(e => addresses.Any(a => a.AddressId == e.AddressId));

            foreach (var e in empl)
            {
                e.AddressId = null;
            }

            foreach (var a in addresses)
            {
                context.Addresses.Remove(a);
            }

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{count} addresses in {town.Name} were deleted";
        }
    }
}
