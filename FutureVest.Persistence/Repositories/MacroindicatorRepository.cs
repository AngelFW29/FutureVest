using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutureVest.Persistence.Repositories
{
    public class MacroindicatorRepository
    {
        private readonly FutureVestContext _context;

        public MacroindicatorRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task<Macroindicator?> AddAsync(Macroindicator macroindicator)
        {
            await _context.Set<Macroindicator>().AddAsync(macroindicator);
            await _context.SaveChangesAsync();
            return macroindicator;
        }
        public async Task<Macroindicator?> UpdateAsync(int id, Macroindicator macroindicator)
        {
            var existing = await _context.Set<Macroindicator>().FindAsync(id);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(macroindicator);
                await _context.SaveChangesAsync();
                return existing;
            }
            return null;
        }
        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Set<Macroindicator>().FindAsync(id);

            if (existing != null)
            {
                _context.Set<Macroindicator>().Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Macroindicator>> GetAll()
        {
            return await _context.Set<Macroindicator>().ToListAsync();
        }
        public async Task<Macroindicator?> GetById(int id)
        {
            return await _context.Set<Macroindicator>().FindAsync(id);
        }
        public IQueryable<Macroindicator> GetAllQry()
        {
            return _context.Set<Macroindicator>().AsQueryable();
        }
    }

}
