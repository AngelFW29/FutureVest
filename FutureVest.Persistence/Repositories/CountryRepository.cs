using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutureVest.Persistence.Repositories
{
    public class CountryRepository
    {
        private readonly FutureVestContext _context;
        public CountryRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task<Country?> AddAsync(Country country)
        {
            await _context.Set<Country>().AddAsync(country);
            await _context.SaveChangesAsync();
            return country;
        }
        public async Task<Country?> UpdateAsync(int id, Country country)
        {
            var existing = await _context.Set<Country>().FindAsync(id);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(country);
                await _context.SaveChangesAsync();
                return existing;
            }
            return null;
        }
        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Set<Country>().FindAsync(id);

            if (existing != null)
            {
                _context.Set<Country>().Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Country>> GetAll()
        {
            return await _context.Set<Country>().ToListAsync();
        }
        public async Task<Country?> GetById(int id)
        {
            return await _context.Set<Country>().FindAsync(id);
        }
        public IQueryable<Country> GetAllQry()
        {
            return _context.Set<Country>().AsQueryable();
        }
    }
}