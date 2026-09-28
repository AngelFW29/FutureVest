using FutureVest.Application.Common;
using FutureVest.Application.Dtos.ReturnRateSetting;
using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using FutureVest.Persistence.Repositories;

namespace FutureVest.Application.Services
{
    public class ReturnRateSettingService
    {
        public readonly ReturnRateSettingRepository _returnRateSettingRepository;
        public ReturnRateSettingService(FutureVestContext futureVestContext)
        {
            _returnRateSettingRepository = new ReturnRateSettingRepository(futureVestContext);
        }

        public async Task<ReturnRateSettingDto?> GetAll()
        {
            try
            {
                var settings = await _returnRateSettingRepository.GetAll();
                var existing = settings.FirstOrDefault();

                if (existing == null)
                {
                    return null; //ojito

                }
                return new ReturnRateSettingDto
                {
                    Id = existing.Id,
                    MinReturnRate = existing.MinReturnRate,
                    MaxReturnRate = existing.MaxReturnRate
                };
            }
            catch (Exception)
            {
                return new ReturnRateSettingDto { Id = 0, MinReturnRate = 2, MaxReturnRate = 15 };

            }
        }
        public async Task<ServiceResponse> SaveAsync(ReturnRateSettingDto returnRateSettingDto)
        {
            try
            {
                if (returnRateSettingDto.MinReturnRate >= returnRateSettingDto.MaxReturnRate)
                {
                    return ServiceResponse.Fail("La tasa mínima debe ser menor que la tasa máxima");
                }

                var settings = await _returnRateSettingRepository.GetAll();
                var existing = settings.FirstOrDefault();

                if (existing == null)
                {
                    ReturnRateSetting entity = new()
                    {
                        Id = 0,
                        MinReturnRate = returnRateSettingDto.MinReturnRate,
                        MaxReturnRate = returnRateSettingDto.MaxReturnRate
                    };

                    var created = await _returnRateSettingRepository.AddAsync(entity);
                    return created != null
                        ? ServiceResponse.Ok()
                        : ServiceResponse.Fail("No se pudo guardar la configuración.");
                }
                else
                {
                    ReturnRateSetting entity = new()
                    {
                        Id = existing.Id,
                        MinReturnRate = returnRateSettingDto.MinReturnRate,
                        MaxReturnRate = returnRateSettingDto.MaxReturnRate
                    };

                    var update = await _returnRateSettingRepository.UpdateAsync(entity.Id, entity);
                    return update != null
                        ? ServiceResponse.Ok()
                        : ServiceResponse.Fail("No se pudo guardar la configuración.");
                }
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado");
            }
        }
    }
}
