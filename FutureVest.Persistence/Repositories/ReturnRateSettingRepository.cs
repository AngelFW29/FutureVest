using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutureVest.Persistence.Repositories
{
    public class ReturnRateSettingRepository
    {
        private readonly FutureVestContext _context;

        public ReturnRateSettingRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task<ReturnRateSetting?> AddAsync(ReturnRateSetting returnRateSetting)
        {
            await _context.Set<ReturnRateSetting>().AddAsync(returnRateSetting);
            await _context.SaveChangesAsync();
            return returnRateSetting;
        }
        public async Task<ReturnRateSetting?> UpdateAsync(int id, ReturnRateSetting returnRateSetting)
        {
            var existing = await _context.Set<ReturnRateSetting>().FindAsync(id);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(returnRateSetting);
                await _context.SaveChangesAsync();
                return existing;
            }
            return null;
        }
        public async Task<List<ReturnRateSetting>> GetAll()
        {
            return await _context.Set<ReturnRateSetting>().ToListAsync();
        }
    }
}