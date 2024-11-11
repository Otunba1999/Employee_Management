using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class DepartmentRepository(DataContext context) : IGenericRepository<Department>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var dep = await context.Departments.FindAsync(id);
            if(dep is null) return NotFound();
            context.Departments.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<List<Department>> GetAll()
        {
            return await context.Departments.AsNoTracking().Include(d => d.GeneralDepartment).ToListAsync();
        }

        public async Task<Department> GetById(int id)
        {
            return await context.Departments.FindAsync(id);
        }

        public async Task<GeneralResponse> Insert(Department entity)
        {
            if(!await CheckName(entity.Name!)) return new(false, "Department already exists");
            context.Departments.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Department entity)
        {
            var dep = await context.Departments.FindAsync(entity.Id);
            if(dep is null) return NotFound();
            dep.Name = entity.Name;
            dep.GeneralDepartmentId = entity.GeneralDepartmentId;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry department not found");
        private static GeneralResponse Success() => new(true, "Process Completed]");
        private async Task Commit() => await context.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var dep = await context.Departments.FirstOrDefaultAsync(d => d.Name!.ToLower().Equals(name.ToLower()));
            return dep is null;
        }
    }
}