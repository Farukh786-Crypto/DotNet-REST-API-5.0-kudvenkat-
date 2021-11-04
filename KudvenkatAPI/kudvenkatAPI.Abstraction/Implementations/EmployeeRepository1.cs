using DAL;
using kudvenkatAPI.Abstraction.Abstraction;
using KudvenkatAPI.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kudvenkatAPI.Abstraction.Implementations
{
    public class EmployeeRepository1 : IEmployeeRepository1
    {
        private readonly DataContext dataContext;

        public EmployeeRepository1(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await dataContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            // inside Employee class we pass Department class also so use include
            return await dataContext.Employees.Include(a => a.Department).FirstOrDefaultAsync<Employee>(a => a.EmployeeId == employeeId);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await dataContext.Employees.FirstOrDefaultAsync(a => a.Email == email);
        }
        public async Task<Employee> AddEmployee(Employee employee)
        {
            if(employee.Department!=null) 
            {
                // ignoring department class so u dont get error on identity set off of departmentId
                dataContext.Entry(employee.Department).State = EntityState.Unchanged;
            }
            var result = await dataContext.Employees.AddAsync(employee);
            await dataContext.SaveChangesAsync();
            return result.Entity;
        }
      
        public async Task DeleteEmployee(int employeeId)
        {
            var result = await dataContext.Employees.FirstOrDefaultAsync<Employee>(a => a.EmployeeId == employeeId);
            if(result!=null)
            {
                dataContext.Employees.Remove(result);
                await dataContext.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Employee>> Search(string name, Gender? gender)
        {
            IQueryable<Employee> query = dataContext.Employees;

            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(a=>a.FirstName.Contains(name)||a.LastName.Contains(name));
            }

            if(gender!=null)
            {
                query = query.Where(a=>a.Gender==gender);
            }
            return await query.ToListAsync();
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result = await dataContext.Employees.FirstOrDefaultAsync(a=>a.EmployeeId==employee.EmployeeId);

            if(result!=null)
            {
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Email = employee.Email;
                result.DateOfBirth = employee.DateOfBirth;
                result.Gender = employee.Gender;
                /*result.DepartmentId = employee.DepartmentId;*/
                if(employee.DepartmentId !=0)
                {
                    result.DepartmentId = employee.DepartmentId;
                }
                else if(employee.Department != null)
                {
                    result.DepartmentId = employee.Department.DepartmentId;
                }
                result.PhotoPath = employee.PhotoPath;

                await dataContext.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
