using FutureVest.Application.Common;
using FutureVest.Application.Dtos.Macroindicator;
using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using FutureVest.Persistence.Repositories;

namespace FutureVest.Application.Services
{
    public class MacroindicatorService
    {
        private readonly MacroindicatorRepository _macroindicatorRepository;

        public MacroindicatorService(FutureVestContext futureVestContext)
        {
            _macroindicatorRepository = new MacroindicatorRepository(futureVestContext);
        }

        public async Task<ServiceResponse> AddAsync(MacroindicatorDto macroindicatorDto)
        {
            try
            {
                var macroindicators = await _macroindicatorRepository.GetAll();
                decimal currentWeight = macroindicators.Sum(m => m.Weight);

                if (currentWeight + macroindicatorDto.Weight > 1)
                {
                    return ServiceResponse.Fail("Ajuste los pesos de los macroindicadores hasta que su suma sea igual a 1.");
                }

                Macroindicator entity = new()
                {
                    Id = 0,
                    Name = macroindicatorDto.Name,
                    Weight = macroindicatorDto.Weight,
                    IsHigherBetter = macroindicatorDto.IsHigherBetter
                };

                Macroindicator? returnEntity = await _macroindicatorRepository.AddAsync(entity);
                return returnEntity != null
                    ? ServiceResponse.Ok()
                    : ServiceResponse.Fail("No se pudo crear el macroindicador.");
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(MacroindicatorDto macroindicatorDto)
        {
            try
            {
                var macroindicators = await _macroindicatorRepository.GetAll();

                decimal weightOfOthers = macroindicators
                    .Where(m => m.Id != macroindicatorDto.Id)
                    .Sum(m => m.Weight);

                if (weightOfOthers + macroindicatorDto.Weight > 1)
                {
                    return ServiceResponse.Fail("Ajuste los pesos de los macroindicadores hasta que su suma sea igual a 1");
                }

                Macroindicator entity = new()
                {
                    Id = macroindicatorDto.Id,
                    Name = macroindicatorDto.Name,
                    Weight = macroindicatorDto.Weight,
                    IsHigherBetter = macroindicatorDto.IsHigherBetter
                };

                Macroindicator? returnEntity = await _macroindicatorRepository.UpdateAsync(entity.Id, entity);
                return returnEntity != null
                ? ServiceResponse.Ok()
                : ServiceResponse.Fail("No se pudo editar el macroindicador");
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado");
            }
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            try
            {
                await _macroindicatorRepository.DeleteAsync(id);
                return ServiceResponse.Ok();
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("No se puede eliminar, el macroindicador tiene indicadores de países registrados");
            }
        }

        public async Task<List<MacroindicatorDto>> GetAll()
        {
            try
            {
                var listEntities = await _macroindicatorRepository.GetAll();

                var listMacroindicatorDto = listEntities.Select(s => new MacroindicatorDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Weight = s.Weight,
                    IsHigherBetter = s.IsHigherBetter

                }).ToList();
                return listMacroindicatorDto;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<MacroindicatorDto?> GetById(int id)
        {
            try
            {
                var entity = await _macroindicatorRepository.GetById(id);
                if (entity == null) return null;

                var dto = new MacroindicatorDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Weight = entity.Weight,
                    IsHigherBetter = entity.IsHigherBetter
                };
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}