using FutureVest.Application.Common;
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

        public async Task<ServiceResponse> AddAsync(CountryDto countryDto)
        {
            try
            {
                var countries = await _countryRepository.GetAll();
                bool isDuplicate = countries.Any(c => c.IsoCode == countryDto.IsoCode);

                if (isDuplicate)
                {
                    return ServiceResponse.Fail("Ya existe un país registrado con ese código ISO.");
                }

                Country country = new() { Id = 0, Name = countryDto.Name, IsoCode = countryDto.IsoCode };

                Country? returnCountry = await _countryRepository.AddAsync(country);
                return returnCountry != null
                    ? ServiceResponse.Ok()
                    : ServiceResponse.Fail("No se pudo crear el país");
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(CountryDto countryDto)
        {
            try
            {
                var countries = await _countryRepository.GetAll();
                bool isDuplicate = countries.Any(c => c.IsoCode == countryDto.IsoCode && c.Id != countryDto.Id);

                if (isDuplicate)
                {
                    return ServiceResponse.Fail("Ya existe un país registrado con ese código ISO");
                }

                Country country = new() { Id = countryDto.Id, Name = countryDto.Name, IsoCode = countryDto.IsoCode };

                Country? returnCountry = await _countryRepository.UpdateAsync(country.Id, country);
                return returnCountry != null
                    ? ServiceResponse.Ok()
                    : ServiceResponse.Fail("No se pudo editar el país");
            }
            catch (Exception)
            {
                return ServiceResponse.Fail("Ocurrió un error inesperado");
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
