using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutureVest.Persistence.Repositories
{
    public class CountryIndicatorRepository
    {
        private readonly FutureVestContext _context;
        public CountryIndicatorRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task<CountryIndicator?> AddAsync(CountryIndicator countryIndicator)
        {
            await _context.Set<CountryIndicator>().AddAsync(countryIndicator);
            await _context.SaveChangesAsync();
            return countryIndicator;
        }
        public async Task<CountryIndicator?> UpdateAsync(int id, CountryIndicator countryIndicator)
        {
            var existing = await _context.Set<CountryIndicator>().FindAsync(id);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(countryIndicator);
                await _context.SaveChangesAsync();
                return existing;
            }
            return null;
        }
        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Set<CountryIndicator>().FindAsync(id);

            if (existing != null)
            {
                _context.Set<CountryIndicator>().Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<CountryIndicator>> GetAll()
        {
            return await _context.Set<CountryIndicator>().ToListAsync();
        }
        public async Task<CountryIndicator?> GetById(int id)
        {
            return await _context.Set<CountryIndicator>().FindAsync(id);
        }
        public IQueryable<CountryIndicator> GetAllQry()
        {
            return _context.Set<CountryIndicator>().AsQueryable();
        }
    }
}