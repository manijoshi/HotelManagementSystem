using HotelBookingSystem.Application.DTO.CityDTO;
using HotelBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("createcity")]
        public async Task<IActionResult> CreateCity(CityRequest request)
        {
            var city = await _cityService.CreateCityAsync(request);
            return CreatedAtAction(nameof(GetCityById), new { id = city.CityId }, city);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _cityService.GetCityByIdAsync(id);
            return Ok(city);
        }

        [HttpGet("{id}/with-hotels")]
        public async Task<IActionResult> GetCityWithHotelsById(int id)
        {
            var city = await _cityService.GetCityByIdWithHotelsAsync(id);
            return Ok(city);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityRequest request)
        {

            var city = await _cityService.UpdateCityAsync(id, request);
            return Ok(city);

        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {

            await _cityService.DeleteCityAsync(id);
            return NoContent();

        }

        [HttpGet("popular-cities")]
        public async Task<IActionResult> GetPopularCities([FromQuery] int limit = 5)
        {
            var popularCities = await _cityService.GetPopularCitiesAsync(limit);
            return Ok(popularCities);
        }
    }
}
