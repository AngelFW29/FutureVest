using FutureVest.Application.Common;
using FutureVest.Application.Dtos.Country;
using FutureVest.Application.Dtos.CountryIndicator;
using FutureVest.Application.Dtos.Macroindicator;
using FutureVest.Application.Dtos.SimulationMacroindicator;
using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using FutureVest.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FutureVest.Application.Services
{
    public class SimulationMacroindicatorService
    {
        public readonly SimulationMacroindicatorRepository _simulationMacroindicatorRepository;
        private readonly MacroindicatorService _macroindicatorService;

        public SimulationMacroindicatorService(FutureVestContext futureVestContext)
        {
            _simulationMacroindicatorRepository = new SimulationMacroindicatorRepository(futureVestContext);
            _macroindicatorService = new MacroindicatorService(futureVestContext);
        }
        public async Task<List<MacroindicatorDto>> GetAvailableMacroindicators()
        {
            try
            {
                var allMacroindicators = await _macroindicatorService.GetAll();
                var simulationEntries = await _simulationMacroindicatorRepository.GetAll();

                var usedIds = simulationEntries.Select(s => s.MacroindicatorId).ToList();

                return allMacroindicators
                    .Where(m => !usedIds.Contains(m.Id))
                    .ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<ServiceResponse> AddAsync(SimulationMacroindicatorDto simulationMacroindicatorDto)
        {
            try
            {
                var entries = await _simulationMacroindicatorRepository.GetAll();

                bool alreadyAdded = entries.Any(s => s.MacroindicatorId == simulationMacroindicatorDto.MacroindicatorId);
                if (alreadyAdded)
                {
                    return ServiceResponse.Fail("Este macroindicador ya fue agregado a la simulación.");
                }

                decimal currentWeight = entries.Sum(s => s.Weight);
                if (currentWeight + simulationMacroindicatorDto.Weight > 1)
                {
                    return ServiceResponse.Fail("El peso al sumarlo con los demás macroindicadores de la simulación no debe superar 1.");
                }

                RankingSimulationMacroindicator entity = new()
                {
                    Id = 0,
                    MacroindicatorId = simulationMacroindicatorDto.MacroindicatorId,
                    Weight = simulationMacroindicatorDto.Weight
                };

                var returnEntity = await _simulationMacroindicatorRepository.AddAsync(entity);
                return returnEntity != null
                    ? ServiceResponse.Ok()
                    : ServiceResponse.Fail("No se pudo agregar el macroindicador a la simulación.");
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado.");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(SimulationMacroindicatorDto simulationMacroindicatorDto)
        {
            try
            {
                var existing = await _simulationMacroindicatorRepository.GetById(simulationMacroindicatorDto.Id);
                var entries = await _simulationMacroindicatorRepository.GetAll();

                if (existing == null)
                {
                    return ServiceResponse.Fail("El indicador que intenta editar no existe.");
                }

                decimal weightOfOthers = entries
                    .Where(s => s.Id != simulationMacroindicatorDto.Id)
                    .Sum(s => s.Weight);

                if (weightOfOthers + simulationMacroindicatorDto.Weight > 1)
                {
                    return ServiceResponse.Fail("El peso al sumarlo con los demás macroindicadores de la simulación no debe superar 1.");
                }

                RankingSimulationMacroindicator entity = new()
                {
                    Id = existing.Id,
                    MacroindicatorId = existing.MacroindicatorId,
                    Weight = simulationMacroindicatorDto.Weight
                };
                var returnEntity = await _simulationMacroindicatorRepository.UpdateAsync(entity.Id, entity);
                return returnEntity != null
                    ? ServiceResponse.Ok()
                    : ServiceResponse.Fail("No se pudo editar el macroindicador a la simulación.");
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado.");
            }
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            try
            {
                await _simulationMacroindicatorRepository.DeleteAsync(id);
                return ServiceResponse.Ok();
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado");
            }
        }

        public async Task<SimulationMacroindicatorDto?> GetById(int id)
        {
            try
            {
                var qry = _simulationMacroindicatorRepository.GetAllQry();

                var entity = await qry
                .Include(s => s.Macroindicator)
                .FirstOrDefaultAsync(s => s.Id == id);

                if (entity == null) return null;

                var dto = new SimulationMacroindicatorDto()
                {
                    Id = entity.Id,
                    MacroindicatorId = entity.MacroindicatorId,
                    Weight = entity.Weight,
                    Macroindicator = entity.Macroindicator == null ? null : new MacroindicatorDto()
                    {
                        Id = entity.Macroindicator.Id,
                        Name = entity.Macroindicator.Name,
                        Weight = entity.Macroindicator.Weight,
                        IsHigherBetter = entity.Macroindicator.IsHigherBetter,
                    }
                };
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SimulationMacroindicatorDto>> GetAllInclude()
        {
            try
            {
                var listEntitiesQry = _simulationMacroindicatorRepository.GetAllQry();

                var listEntities = await listEntitiesQry
                    .Include(s => s.Macroindicator)
                    .ToListAsync();

                var listEntityDtos = listEntities.Select(s =>
                new SimulationMacroindicatorDto()
                {
                    Id = s.Id,
                    MacroindicatorId = s.MacroindicatorId,                 
                    Weight = s.Weight,
                    Macroindicator = s.Macroindicator == null ? null : new MacroindicatorDto()
                    {
                        Id = s.Macroindicator.Id,
                        Name = s.Macroindicator.Name,
                        Weight = s.Macroindicator.Weight,
                        IsHigherBetter = s.Macroindicator.IsHigherBetter,
                    },
                }).ToList();

                return listEntityDtos;
            }
            catch
            {
                return [];
            }
        }

    }
}