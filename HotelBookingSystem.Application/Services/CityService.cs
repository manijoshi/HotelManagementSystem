using AutoMapper;
using HotelBookingSystem.Application.DTO.CityDTO;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;

namespace HotelBookingSystem.Application.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CityService(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<CityResponse> CreateCityAsync(CityRequest request)
        {
            var city = _mapper.Map<City>(request);
            city.CreatedAt = DateTime.UtcNow;
            city.UpdatedAt = DateTime.UtcNow;

            await _cityRepository.AddAsync(city);

            return _mapper.Map<CityResponse>(city);
        }

        public async Task<CityResponse> GetCityByIdAsync(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found");

            return _mapper.Map<CityResponse>(city);
        }

        public async Task<CityResponseWithNumberOfHotels> GetCityByIdWithHotelsAsync(int id)
        {
            var city = await _cityRepository.GetByIdWithHotelsAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found");

            return _mapper.Map<CityResponseWithNumberOfHotels>(city);
        }

        public async Task<CityResponse> UpdateCityAsync(int id, CityRequest request)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException("City not found");

            _mapper.Map(request, city);
            city.UpdatedAt = DateTime.UtcNow;

            await _cityRepository.UpdateAsync(city);

            return _mapper.Map<CityResponse>(city);
        }

        public async Task DeleteCityAsync(int id)
        {

            await _cityRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CityResponse>> GetPopularCitiesAsync(int limit)
        {
            var cities = await _cityRepository.GetPopularCitiesAsync(limit);
            return _mapper.Map<IEnumerable<CityResponse>>(cities);
                
        }
    }
}
