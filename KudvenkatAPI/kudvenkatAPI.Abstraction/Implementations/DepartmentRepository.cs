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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext dataContext;

        public DepartmentRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task<Department> GetDepartment(int DepartmentId)
        {
            return await dataContext.Departments.FirstOrDefaultAsync(a=>a.DepartmentId==DepartmentId);
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await dataContext.Departments.ToListAsync();
        }
    }
}
