using AutoMapper;
using FluentAssertions;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Application.MappingProfiles;
using HotelBookingSystem.Application.Services;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace HotelBookingSystem.Tests.ServiceTests
{
    public class HotelServiceTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly HotelService _hotelService;

        public HotelServiceTests()
        {
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _cityRepositoryMock = new Mock<ICityRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HotelProfile>();
                cfg.AddProfile<RoomProfile>();
                cfg.AddProfile<GuestReviewProfile>();  
            });

            _mapper = config.CreateMapper();
            _hotelService = new HotelService(_hotelRepositoryMock.Object, _mapper, _cityRepositoryMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task CreateHotelAsync_ShouldReturnHotelResponse()
        {
            var hotelRequest = new HotelRequest
            {
                Name = "Test Hotel",
                CityId = 1,
                Description = "Test Description"
            };

            var city = new City { CityId = 1 };
            var hotel = new Hotel
            {
                HotelId = 1,
                Name = "Test Hotel",
                CityId = 1,
                Description = "Test Description",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _cityRepositoryMock.Setup(repo => repo.ExistsAsync(hotel.CityId)).ReturnsAsync(true);
            _hotelRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Hotel>())).ReturnsAsync(hotel);

            var result = await _hotelService.CreateHotelAsync(hotelRequest);

            result.Should().NotBeNull();
            result.Name.Should().Be(hotel.Name);
            result.Description.Should().Be(hotel.Description);
            _hotelRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Hotel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateHotelAsync_ShouldReturnUpdatedHotelResponse_WhenHotelExists()
        {
            var hotelId = 1;
            var hotelRequest = new HotelRequest
            {
                Name = "Updated Hotel",
                Description = "Updated Description"
            };

            var hotel = new Hotel
            {
                HotelId = hotelId,
                Name = "Test Hotel",
                Description = "Test Description",
                UpdatedAt = DateTime.UtcNow
            };

            _hotelRepositoryMock.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync(hotel);
            _hotelRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Hotel>())).ReturnsAsync(hotel);

            var result = await _hotelService.UpdateHotelAsync(hotelId, hotelRequest);

            result.Should().NotBeNull();
            result.Name.Should().Be(hotelRequest.Name);
            result.Description.Should().Be(hotelRequest.Description);
            _hotelRepositoryMock.Verify(repo => repo.GetByIdAsync(hotelId), Times.Once);
            _hotelRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Hotel>()), Times.Once);
        }

        [Fact]
        public async Task DeleteHotelAsync_ShouldCallDeleteAsync()
        {
            var hotelId = 1;

            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(hotelId)).ReturnsAsync(true);
            _hotelRepositoryMock.Setup(repo => repo.DeleteAsync(hotelId)).ReturnsAsync(1);

            await _hotelService.DeleteHotelAsync(hotelId);

            _hotelRepositoryMock.Verify(repo => repo.DeleteAsync(hotelId), Times.Once);
        }

        [Fact]
        public async Task GetHotelByIdAsync_ShouldReturnHotelResponse_WhenHotelExists()
        {
            var hotelId = 1;
            var hotel = new Hotel { HotelId = hotelId, Name = "Test Hotel", Description = "Test Description" };

            _hotelRepositoryMock.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync(hotel);

            var result = await _hotelService.GetHotelByIdAsync(hotelId);

            result.Should().NotBeNull();
            result.Name.Should().Be(hotel.Name);
            result.Description.Should().Be(hotel.Description);
            _hotelRepositoryMock.Verify(repo => repo.GetByIdAsync(hotelId), Times.Once);
        }

        [Fact]
        public async Task GetHotelByIdAsync_ShouldThrowKeyNotFoundException_WhenHotelDoesNotExist()
        {
            var hotelId = 1;

            _hotelRepositoryMock.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync((Hotel)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _hotelService.GetHotelByIdAsync(hotelId));
            _hotelRepositoryMock.Verify(repo => repo.GetByIdAsync(hotelId), Times.Once);
        }

        [Fact]
        public async Task SearchHotelsAsync_ShouldReturnPaginatedListOfHotels()
        {
            var searchParameters = new HotelSearchParameters
            {
                Page = 1,
                PageSize = 10,
            };

            var hotels = new List<Hotel>
            {
                new Hotel { HotelId = 1, Rooms = new List<Room> { new Room { RoomId = 1, Bookings = new List<Booking>() } } }
            };

            var totalResults = hotels.Count;
            _hotelRepositoryMock.Setup(repo => repo.SearchAsync(searchParameters)).ReturnsAsync((hotels, totalResults));

            var result = await _hotelService.SearchHotelsAsync(searchParameters);

            result.Should().NotBeNull();
            result.TotalResults.Should().Be(totalResults);
            result.Items.Should().HaveCount(hotels.Count);
        }

        [Fact]
        public async Task GetFeaturedDealsAsync_ShouldReturnHotelsWithFeaturedDeals()
        {
            var limit = 5;
            var hotels = new List<Hotel>
            {
                new Hotel { HotelId = 1, Rooms = new List<Room> { new Room { RoomId = 1, FeaturedDeal = true, DiscountedPrice = 100 } } }
            };

            _hotelRepositoryMock.Setup(repo => repo.GetFeaturedDealsAsync(limit)).ReturnsAsync(hotels);

            var result = await _hotelService.GetFeaturedDealsAsync(limit);

            result.Should().NotBeNull();
            result.Should().ContainSingle();
            result.First().HotelId.Should().Be(hotels.First().HotelId);
        }

        [Fact]
        public async Task GetRecentHotelsAsync_ShouldReturnHotelsVisitedByUser()
        {
            var userId = 1;
            var limit = 5;

            var hotels = new List<Hotel>
            {
                new Hotel { HotelId = 1, Bookings = new List<Booking> { new Booking { UserId = userId, CheckOutDate = DateTime.UtcNow.AddDays(-1) } } }
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "mock");
            var user = new ClaimsPrincipal(claimsIdentity);

            _httpContextAccessorMock.Setup(_ => _.HttpContext.User).Returns(user);
            _hotelRepositoryMock.Setup(repo => repo.GetRecientHotelsAsync(userId, limit)).ReturnsAsync(hotels);

            var result = await _hotelService.GetRecentHotelsAsync(limit);

            result.Should().NotBeNull();
            result.Should().ContainSingle();
            result.First().HotelId.Should().Be(hotels.First().HotelId);
        }

        [Fact]
        public async Task GetHotelByIdWithReviewsAsync_ShouldReturnHotelResponseWithReviews_WhenHotelExists()
        {
            var hotelId = 1;
            var hotelWithReviews = new Hotel
            {
                HotelId = hotelId,
                Name = "Test Hotel",
                GuestReviews = new List<GuestReview> { new GuestReview { GuestReviewId = 1, Rating = 5, Comment = "Excellent!" } }
            };

            _hotelRepositoryMock.Setup(repo => repo.GetByIdWithReviewsAsync(hotelId)).ReturnsAsync(hotelWithReviews);

            var result = await _hotelService.GetHotelByIdWithReviewsAsync(hotelId);

            result.Should().NotBeNull();
            result.HotelId.Should().Be(hotelId);
            result.GuestReviews.Should().ContainSingle();
            result.GuestReviews.First().Comment.Should().Be("Excellent!");
            _hotelRepositoryMock.Verify(repo => repo.GetByIdWithReviewsAsync(hotelId), Times.Once);
        }

        [Fact]
        public async Task GetHotelByIdWithReviewsAsync_ShouldThrowKeyNotFoundException_WhenHotelDoesNotExist()
        {
            var hotelId = 1;

            _hotelRepositoryMock.Setup(repo => repo.GetByIdWithReviewsAsync(hotelId)).ReturnsAsync((Hotel)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _hotelService.GetHotelByIdWithReviewsAsync(hotelId));
            _hotelRepositoryMock.Verify(repo => repo.GetByIdWithReviewsAsync(hotelId), Times.Once);
        }
    }
}
