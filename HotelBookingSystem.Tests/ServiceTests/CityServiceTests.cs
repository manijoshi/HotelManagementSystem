using AutoMapper;
using FluentAssertions;
using HotelBookingSystem.Application.DTO.CityDTO;
using HotelBookingSystem.Application.MappingProfiles;
using HotelBookingSystem.Application.Services;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using Moq;

namespace HotelBookingSystem.Tests.ServiceTests
{
    public class CityServiceTests
    {
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CityService _cityService;

        public CityServiceTests()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CityProfile>();
            });

            _mapper = configuration.CreateMapper();
            _cityService = new CityService(_cityRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task CreateCityAsync_ShouldReturnCityResponse()
        {
            var cityRequest = new CityRequest
            {
                Name = "Test City",
                Country = "Test Country"
            };

            var city = new City
            {
                CityId = 1,
                Name = "Test City",
                Country = "Test Country",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _cityRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<City>())).ReturnsAsync(city);

            var result = await _cityService.CreateCityAsync(cityRequest);

            result.Should().NotBeNull();
            result.Name.Should().Be(city.Name);
            result.Country.Should().Be(city.Country);
            _cityRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<City>()), Times.Once);
        }

        [Fact]
        public async Task GetCityByIdAsync_ShouldReturnCityResponse_WhenCityExists()
        {
            var cityId = 1;
            var city = new City { CityId = cityId, Name = "Test City", Country = "Test Country" };

            _cityRepositoryMock.Setup(repo => repo.GetByIdAsync(cityId)).ReturnsAsync(city);

            var result = await _cityService.GetCityByIdAsync(cityId);

            result.Should().NotBeNull();
            result.Name.Should().Be(city.Name);
            result.Country.Should().Be(city.Country);
            _cityRepositoryMock.Verify(repo => repo.GetByIdAsync(cityId), Times.Once);
        }

        [Fact]
        public async Task GetCityByIdAsync_ShouldThrowKeyNotFoundException_WhenCityDoesNotExist()
        {
            var cityId = 1;

            _cityRepositoryMock.Setup(repo => repo.GetByIdAsync(cityId)).ReturnsAsync((City)null);

            Func<Task> act = async () => await _cityService.GetCityByIdAsync(cityId);
            await act.Should().ThrowAsync<KeyNotFoundException>();
            _cityRepositoryMock.Verify(repo => repo.GetByIdAsync(cityId), Times.Once);
        }

        [Fact]
        public async Task UpdateCityAsync_ShouldReturnUpdatedCityResponse_WhenCityExists()
        {
            var cityId = 1;
            var cityRequest = new CityRequest { Name = "Updated City", Country = "Updated Country" };
            var city = new City { CityId = cityId, Name = "Test City", Country = "Test Country", UpdatedAt = DateTime.UtcNow };

            _cityRepositoryMock.Setup(repo => repo.GetByIdAsync(cityId)).ReturnsAsync(city);
            _cityRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<City>())).ReturnsAsync(city);

            var result = await _cityService.UpdateCityAsync(cityId, cityRequest);

            result.Should().NotBeNull();
            result.Name.Should().Be(cityRequest.Name);
            result.Country.Should().Be(cityRequest.Country);
            _cityRepositoryMock.Verify(repo => repo.GetByIdAsync(cityId), Times.Once);
            _cityRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<City>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCityAsync_ShouldCallDeleteAsync()
        {
            var cityId = 1;

            _cityRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1);

            await _cityService.DeleteCityAsync(cityId);

            _cityRepositoryMock.Verify(repo => repo.DeleteAsync(cityId), Times.Once);
        }

        [Fact]
        public async Task GetPopularCitiesAsync_ShouldReturnPopularCities()
        {
            var cities = new List<City>
                {
                    new City { CityId = 1, Name = "Popular City 1" },
                    new City { CityId = 2, Name = "Popular City 2" }
                };

            _cityRepositoryMock.Setup(repo => repo.GetPopularCitiesAsync(It.IsAny<int>())).ReturnsAsync(cities);

            var result = await _cityService.GetPopularCitiesAsync(2);

            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Popular City 1");
            result.Last().Name.Should().Be("Popular City 2");
            _cityRepositoryMock.Verify(repo => repo.GetPopularCitiesAsync(2), Times.Once);
        }

        [Fact]
        public async Task GetPopularCitiesAsync_ShouldReturnEmptyList_WhenNoCitiesArePopular()
        {
            _cityRepositoryMock.Setup(repo => repo.GetPopularCitiesAsync(It.IsAny<int>())).ReturnsAsync(new List<City>());

            var result = await _cityService.GetPopularCitiesAsync(0);

            result.Should().BeEmpty();
            _cityRepositoryMock.Verify(repo => repo.GetPopularCitiesAsync(0), Times.Once);
        }

        [Fact]
        public async Task UpdateCityAsync_ShouldThrowKeyNotFoundException_WhenCityDoesNotExist()
        {
            var cityId = 1;
            var cityRequest = new CityRequest { Name = "Updated City", Country = "Updated Country" };

            _cityRepositoryMock.Setup(repo => repo.GetByIdAsync(cityId)).ReturnsAsync((City)null);

            Func<Task> act = async () => await _cityService.UpdateCityAsync(cityId, cityRequest);
            await act.Should().ThrowAsync<KeyNotFoundException>();
            _cityRepositoryMock.Verify(repo => repo.GetByIdAsync(cityId), Times.Once);
        }


    }
}
