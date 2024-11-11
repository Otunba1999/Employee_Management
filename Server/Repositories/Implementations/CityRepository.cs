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
    public class CityRepository(DataContext context) : IGenericRepository<City>
    {
         public async Task<GeneralResponse> DeleteById(int id)
        {
            var branch = await context.Cities.FindAsync(id);
            if(branch is null) return NotFound();
            context.Cities.Remove(branch);
            await Commit();
            return Success();
        }

        public async Task<List<City>> GetAll()
        {
            return await context.Cities.AsNoTracking().Include(c => c.Country).ToListAsync();
        }

        public async Task<City> GetById(int id)
        {
            return await context.Cities.FindAsync(id);
        }

        public async Task<GeneralResponse> Insert(City entity)
        {
            if(!await CheckName(entity.Name!)) return new(false, "Branch already exists");
            context.Cities.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(City entity)
        {
            var city = await context.Cities.FindAsync(entity.Id);
            if(city is null) return NotFound();
            city.Name = entity.Name;
            city.CountryId = entity.CountryId;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry branch not found");
        private static GeneralResponse Success() => new(true, "Process Completed]");
        private async Task Commit() => await context.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var dep = await context.Cities.FirstOrDefaultAsync(d => d.Name!.ToLower().Equals(name.ToLower()));
            return dep is null;
        }
    }
}