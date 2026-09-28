using FutureVest.Application.Common;
using FutureVest.Application.Dtos.Country;
using FutureVest.Application.Dtos.CountryIndicator;
using FutureVest.Application.Dtos.Macroindicator;
using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using FutureVest.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FutureVest.Application.Services
{
    public class CountryIndicatorService
    {
        public readonly CountryIndicatorRepository _countryIndicatorRepository;
        public CountryIndicatorService(FutureVestContext futureVestContext)
        {
            _countryIndicatorRepository = new CountryIndicatorRepository(futureVestContext);
        }

        public async Task<ServiceResponse> AddAsync(CountryIndicatorDto countryIndicatorDto)
        {
            try
            {
                bool isDuplicate = await _countryIndicatorRepository.GetAllQry()
                    .AnyAsync(ci =>
                    ci.CountryId == countryIndicatorDto.CountryId &&
                    ci.MacroindicatorId == countryIndicatorDto.MacroindicatorId &&
                    ci.Year == countryIndicatorDto.Year);

                if (isDuplicate)
                {
                    return ServiceResponse.Fail("Ya existe un indicador asociado a este país, macroindicador y año.");
                }

                CountryIndicator entity = new()
                {
                    Id = 0,
                    Value = countryIndicatorDto.Value,
                    Year = countryIndicatorDto.Year,
                    CountryId = countryIndicatorDto.CountryId,
                    MacroindicatorId = countryIndicatorDto.MacroindicatorId
                };

                CountryIndicator? returnEntity = await _countryIndicatorRepository.AddAsync(entity);
                return returnEntity != null
                    ? ServiceResponse.Ok()
                    : ServiceResponse.Fail("No se pudo crear el indicador.");

            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(CountryIndicatorDto countryIndicatorDto)
        {
            try
            {
                var existing = await _countryIndicatorRepository.GetById(countryIndicatorDto.Id);

                if (existing == null)
                {
                    return ServiceResponse.Fail("El indicador que intenta editar no existe.");
                }

                CountryIndicator entity = new()
                {
                    Id = existing.Id,
                    Value = countryIndicatorDto.Value,
                    Year = existing.Year,
                    CountryId = existing.CountryId,
                    MacroindicatorId = existing.MacroindicatorId
                };

                CountryIndicator? returnEntity = await _countryIndicatorRepository.UpdateAsync(entity.Id, entity);
                return returnEntity != null
                    ? ServiceResponse.Ok()
                    : ServiceResponse.Fail("No se pudo editar el indicador.");
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
                await _countryIndicatorRepository.DeleteAsync(id);
                return ServiceResponse.Ok();
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado");
            }
        }

        public async Task<CountryIndicatorDto?> GetById(int id)
        {
            try
            {
                var entity = await _countryIndicatorRepository.GetById(id);
                if (entity == null) return null;

                var dto = new CountryIndicatorDto()
                {
                    Id = entity.Id,
                    Value = entity.Value,
                    Year = entity.Year,
                    CountryId = entity.CountryId,
                    MacroindicatorId = entity.MacroindicatorId,
                    Country = entity.Country == null ? null : new CountryDto()
                    {
                        Id = entity.Country.Id,
                        Name = entity.Country.Name,
                        IsoCode = entity.Country.IsoCode,
                    },
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

        public async Task<List<CountryIndicatorDto>> GetAllInclude()
        {
            try
            {
                var listEntitiesQry = _countryIndicatorRepository.GetAllQry();

                var listEntities = await listEntitiesQry
                    .Include(ci => ci.Country)
                    .Include(ci => ci.Macroindicator)
                    .ToListAsync();

                var listEntityDtos = listEntities.Select(s =>
                new CountryIndicatorDto()
                {
                    Id = s.Id,
                    Value = s.Value,
                    Year = s.Year,
                    CountryId = s.CountryId,
                    MacroindicatorId = s.MacroindicatorId,
                    Country = s.Country == null ? null : new CountryDto()
                    {
                        Id = s.Country.Id,
                        Name = s.Country.Name,
                        IsoCode = s.Country.IsoCode,
                    },
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
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<CountryIndicatorDto>> GetAllIncludeFilter(int? countryId, int? year)
        {
            try
            {
                IQueryable<CountryIndicator> qry = _countryIndicatorRepository.GetAllQry()
                    .Include(ci => ci.Country)
                    .Include(ci => ci.Macroindicator);
                //    .Where(ci => ci.CountryId == countryId)

                if (countryId.HasValue)
                {
                    qry = qry.Where(ci => ci.CountryId == countryId.Value);
                }

                if (year.HasValue)
                {
                    qry = qry.Where(ci => ci.Year == year.Value);
                }

                var listEntities = await qry.ToListAsync();

                var listEntityDtos = listEntities.Select(s => new CountryIndicatorDto()
                {
                    Id = s.Id,
                    Value = s.Value,
                    Year = s.Year,
                    CountryId = s.CountryId,
                    MacroindicatorId = s.MacroindicatorId,
                    Country = s.Country == null ? null : new CountryDto()
                    {
                        Id = s.Country.Id,
                        Name = s.Country.Name,
                        IsoCode = s.Country.IsoCode,
                    },
                    Macroindicator = s.Macroindicator == null ? null : new MacroindicatorDto()
                    {
                        Id = s.Macroindicator.Id,
                        Name = s.Macroindicator.Name,
                        Weight = s.Macroindicator.Weight,
                        IsHigherBetter = s.Macroindicator.IsHigherBetter,
                    }
                }).ToList();

                return listEntityDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<int>> GetAvailableYearsAsync()
        {
            try
            {
                var indicators = await _countryIndicatorRepository.GetAll();

                return indicators
                    .Select(ci => ci.Year)
                    .Distinct()
                    .OrderByDescending(year => year)
                    .ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}