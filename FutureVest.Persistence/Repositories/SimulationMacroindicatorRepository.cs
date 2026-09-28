using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureVest.Persistence.Repositories
{
    public class SimulationMacroindicatorRepository
    {
        private readonly FutureVestContext _context;
        public SimulationMacroindicatorRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task<RankingSimulationMacroindicator?> AddAsync(RankingSimulationMacroindicator simulationMacroindicator)
        {
            await _context.Set<RankingSimulationMacroindicator>().AddAsync(simulationMacroindicator);
            await _context.SaveChangesAsync();
            return simulationMacroindicator;
        }

        public async Task<RankingSimulationMacroindicator?> UpdateAsync(int id, RankingSimulationMacroindicator simulationMacroindicator)
        {
            var existing = await _context.Set<RankingSimulationMacroindicator>().FindAsync(id);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(simulationMacroindicator);
                await _context.SaveChangesAsync();
                return existing;
            }
            return null;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Set<RankingSimulationMacroindicator>().FindAsync(id);

            if (existing != null)
            {
                _context.Set<RankingSimulationMacroindicator>().Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<RankingSimulationMacroindicator>> GetAll()
        {
            return await _context.Set<RankingSimulationMacroindicator>().ToListAsync();
        }

        public async Task<RankingSimulationMacroindicator?> GetById(int id)
        {
            return await _context.Set<RankingSimulationMacroindicator>().FindAsync(id);
        }

        public IQueryable<RankingSimulationMacroindicator> GetAllQry()
        {
            return _context.Set<RankingSimulationMacroindicator>().AsQueryable();
        }
    }
}