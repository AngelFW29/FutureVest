using FutureVest.Application.Dtos.Country;
using FutureVest.Persistence.Contexts;
using FutureVest.Persistence.Entities;
using FutureVest.Persistence.Repositories;

namespace FutureVest.Application.Services
{
    public class CountryService
    {
        public readonly CountryRepository _countryRepository;

        public CountryService(FutureVestContext futureVestContext)
        {
            _countryRepository = new CountryRepository(futureVestContext);
        }

        public async Task<bool> AddAsync(CountryDto countryDto)
        {
            try
            {
                Country entity = new() { Id = 0, Name = countryDto.Name, IsoCode = countryDto.IsoCode };

                Country? returnEntity = await _countryRepository.AddAsync(entity);
                return returnEntity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(CountryDto countryDto)
        {
            try
            {
                Country entity = new()
                {
                    Id = countryDto.Id,
                    Name = countryDto.Name,
                    IsoCode = countryDto.IsoCode
                };

                Country? returnEntity = await _countryRepository.UpdateAsync(entity.Id, entity);
                return returnEntity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _countryRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CountryDto>> GetAll()
        {
            try
            {
                var listEntities = await _countryRepository.GetAll();

                var listCountryDto = listEntities.Select(s => new CountryDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsoCode = s.IsoCode
                }).ToList();
                return listCountryDto;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<CountryDto?> GetById(int id)
        {
            try
            {
                var entity = await _countryRepository.GetById(id);
                if (entity == null) return null;

                var dto = new CountryDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsoCode = entity.IsoCode
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
