using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class EmployeeRepository(DataContext context) : IGenericRepository<Employee>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
           var emp = await context.Employees.FindAsync(id);
           if(emp is null) return NotFound();
           context.Remove(emp);
           await Commit();
           return Success();
        }

        public async Task<List<Employee>> GetAll()
        {
            var emps = await context.Employees.AsNoTracking()
            .Include(e => e.Town)
            .ThenInclude(t => t.City)
            .ThenInclude(c => c.Country)
            .Include(e => e.Branch)
            .ThenInclude(b => b.Department)
            .ThenInclude(d => d.GeneralDepartment)
            .ToListAsync();
            return emps;
        }

        public async Task<Employee> GetById(int id)
        {
            var emp = await context.Employees.AsNoTracking()
            .Include(e => e.Town)
            .ThenInclude(t => t.City)
            .ThenInclude(c => c.Country)
            .Include(e => e.Branch)
            .ThenInclude(b => b.Department)
            .ThenInclude(d => d.GeneralDepartment)
            .FirstOrDefaultAsync(e => e.Id == id);
            if(emp is null) return null!;
            return emp;
        }

        public async Task<GeneralResponse> Insert(Employee entity)
        {
           if (!await CheckName(entity.Name!)) return new GeneralResponse(false, "Employee already added");
           await context.Employees.AddAsync(entity);
           await Commit();
           return Success();
        }

        public async Task<GeneralResponse> Update(Employee entity)
        {
            var emp = await context.Employees.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if(emp is null) return NotFound();
            emp.Name = entity.Name;
            emp.Address = entity.Address;
            emp.CivilId = entity.CivilId;
            emp.BranchId = entity.BranchId;
            emp.FileNumber = entity.FileNumber;
            emp.Other = entity.Other;
            emp.TelephoneNumber = entity.TelephoneNumber;
            emp.Photo = entity.Photo;
            emp.JobName = entity.JobName;
            emp.TownId = entity.TownId;
            
            await Commit();
            return Success();
        }

         private static GeneralResponse NotFound() => new(false, "Sorry employee not found");
        private static GeneralResponse Success() => new(true, "Process Completed]");
        private async Task Commit() => await context.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var dep = await context.Employees.FirstOrDefaultAsync(d => d.Name!.ToLower().Equals(name.ToLower()));
            return dep is null;
        }
    }
}