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
    public class CountryRepository(DataContext context) : IGenericRepository<Country>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var dep = await context.Countries.FindAsync(id);
            if(dep is null) return NotFound();
            context.Countries.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<List<Country>> GetAll()
        {
            return await context.Countries.ToListAsync();
        }

        public async Task<Country> GetById(int id)
        {
            return await context.Countries.FindAsync(id);
        }

        public async Task<GeneralResponse> Insert(Country entity)
        {
            if(!await CheckName(entity.Name!)) return new(false, "Country already exists");
            context.Countries.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Country entity)
        {
            var dep = await context.Countries.FindAsync(entity.Id);
            if(dep is null) return NotFound();
            dep.Name = entity.Name;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry country not found");
        private static GeneralResponse Success() => new(true, "Process Completed]");
        private async Task Commit() => await context.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var dep = await context.Countries.FirstOrDefaultAsync(d => d.Name!.ToLower().Equals(name.ToLower()));
            return dep is null;
        }
    }
}