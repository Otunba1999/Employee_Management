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
    public class TownRepository(DataContext context) : IGenericRepository<Town>
    {
        
         public async Task<GeneralResponse> DeleteById(int id)
        {
            var town = await context.Towns.FindAsync(id);
            if(town is null) return NotFound();
            context.Towns.Remove(town);
            await Commit();
            return Success();
        }

        public async Task<List<Town>> GetAll()
        {
            return await context.Towns.AsNoTracking().Include(t => t.City).ToListAsync();
        }

        public async Task<Town> GetById(int id)
        {
            return await context.Towns.FindAsync(id);
        }

        public async Task<GeneralResponse> Insert(Town entity)
        {
            if(!await CheckName(entity.Name!)) return new(false, "Branch already exists");
            context.Towns.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Town entity)
        {
            var town = await context.Towns.FindAsync(entity.Id);
            if(town is null) return NotFound();
            town.Name = entity.Name;
            town.CityId = entity.CityId;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry town not found");
        private static GeneralResponse Success() => new(true, "Process Completed]");
        private async Task Commit() => await context.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var dep = await context.Towns.FirstOrDefaultAsync(d => d.Name!.ToLower().Equals(name.ToLower()));
            return dep is null;
        }
    }
}