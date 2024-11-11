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
    public class BranchRepository(DataContext context) : IGenericRepository<Branch>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var branch = await context.Cities.FindAsync(id);
            if(branch is null) return NotFound();
            context.Cities.Remove(branch);
            await Commit();
            return Success();
        }

        public async Task<List<Branch>> GetAll()
        {
            return await context.Branches.AsNoTracking().Include(b => b.Department).ToListAsync();
        }

        public async Task<Branch> GetById(int id)
        {
            return await context.Branches.FindAsync(id);
        }

        public async Task<GeneralResponse> Insert(Branch entity)
        {
            if(!await CheckName(entity.Name!)) return new(false, "Branch already exists");
            context.Branches.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Branch entity)
        {
            var branch = await context.Branches.FindAsync(entity.Id);
            if(branch is null) return NotFound();
            branch.Name = entity.Name;
            branch.DepartmentId = entity.DepartmentId;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry branch not found");
        private static GeneralResponse Success() => new(true, "Process Completed]");
        private async Task Commit() => await context.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var dep = await context.Branches.FirstOrDefaultAsync(d => d.Name!.ToLower().Equals(name.ToLower()));
            return dep is null;
        }
    }
}