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
    public class GeneralDepartmentRepository(DataContext context) : IGenericRepository<GenaralDepartement>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var dep = await context.GenaralDepartements.FindAsync(id);
            if(dep is null) return NotFound();
            context.GenaralDepartements.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<List<GenaralDepartement>> GetAll()
        {
            return await context.GenaralDepartements.ToListAsync();
        }

        public async Task<GenaralDepartement> GetById(int id)
        {
            return await context.GenaralDepartements.FindAsync(id);
        }

        public async Task<GeneralResponse> Insert(GenaralDepartement entity)
        {
            if(!await CheckName(entity.Name!)) return new(false, "Department already exists");
            context.GenaralDepartements.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(GenaralDepartement entity)
        {
            var dep = await context.GenaralDepartements.FindAsync(entity.Id);
            if(dep is null) return NotFound();
            dep.Name = entity.Name;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry department not found");
        private static GeneralResponse Success() => new(true, "Process Completed]");
        private async Task Commit() => await context.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var dep = await context.GenaralDepartements.FirstOrDefaultAsync(d => d.Name!.ToLower().Equals(name.ToLower()));
            return dep is null;
        }
    }
}