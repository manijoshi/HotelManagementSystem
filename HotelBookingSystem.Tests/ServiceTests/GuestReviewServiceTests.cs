using AutoMapper;
using FluentAssertions;
using HotelBookingSystem.Application.DTO.GuestReviewDTO;
using HotelBookingSystem.Application.MappingProfiles;
using HotelBookingSystem.Application.Services;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace HotelBookingSystem.Tests.ServiceTests
{
    public class GuestReviewServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IGuestReviewRepository> _guestReviewRepositoryMock;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly GuestReviewService _guestReviewService;

        public GuestReviewServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GuestReviewProfile>();
            });

            _mapper = config.CreateMapper();

            _guestReviewRepositoryMock = new Mock<IGuestReviewRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _guestReviewService = new GuestReviewService(
                _guestReviewRepositoryMock.Object,
                _mapper,
                _hotelRepositoryMock.Object,
                _httpContextAccessorMock.Object,
                _userRepositoryMock.Object
            );
        }

        [Fact]
        public async Task AddReviewAsync_ShouldReturnGuestReviewResponse_WhenReviewIsSuccessful()
        {
            var reviewRequest = new GuestReviewRequest
            {
                Rating = 5,
                Comment = "Great place!"
            };

            var userId = 1;
            var hotelId = 1;

            var user = new User
            {
                UserId = userId,
                Email = "user@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var review = new GuestReview
            {
                HotelId = 1,
                Rating = 5,
                Comment = "Great place!",
                UserId = userId
            };

            var hotel = new Hotel
            {
                HotelId = 1,
                StarRating = 0
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _guestReviewRepositoryMock.Setup(x => x.AddAsync(It.IsAny<GuestReview>()))
                .ReturnsAsync(review);

            _guestReviewRepositoryMock.Setup(x => x.GetReviewsByHotelIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<GuestReview> { review });

            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(hotel);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var result = await _guestReviewService.AddReviewAsync(hotelId, reviewRequest);

            result.Should().BeOfType<GuestReviewResponse>();
            hotel.StarRating.Should().Be(5);
        }

        [Fact]
        public async Task DeleteReviewAsync_ShouldUpdateHotelRating_WhenReviewIsDeleted()
        {
            var reviewId = 1;
            var hotelId = 1;

            var review = new GuestReview
            {
                GuestReviewId = reviewId,
                HotelId = 1,
                Rating = 4
            };

            var hotel = new Hotel
            {
                HotelId = 1,
                StarRating = 5
            };

            _guestReviewRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(review);

            _guestReviewRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(1);

            _guestReviewRepositoryMock.Setup(x => x.GetReviewsByHotelIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<GuestReview>());

            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(hotel);

            _hotelRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Hotel>()))
                .ReturnsAsync(hotel);

            await _guestReviewService.DeleteReviewAsync(hotelId, reviewId);

            hotel.StarRating.Should().Be(0);
        }

        [Fact]
        public async Task AddReviewAsync_ShouldThrowKeyNotFoundException_WhenHotelDoesNotExist()
        {
            var hotelId = 1;
            var reviewRequest = new GuestReviewRequest { Rating = 5, Comment = "Great place!" };

            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(hotelId)).ReturnsAsync((Hotel)null);

            Func<Task> act = async () => await _guestReviewService.AddReviewAsync(hotelId, reviewRequest);
            await act.Should().ThrowAsync<KeyNotFoundException>();
            _hotelRepositoryMock.Verify(x => x.GetByIdAsync(hotelId), Times.Once);
        }

        [Fact]
        public async Task AddReviewAsync_ShouldThrowUnauthorizedAccessException_WhenUserIsNotAuthenticated()
        {
            var hotelId = 1;
            var reviewRequest = new GuestReviewRequest { Rating = 5, Comment = "Great place!" };
            var hotel = new Hotel { HotelId = hotelId, StarRating = 0 };

            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns((Claim)null); 

            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(hotelId))
                .ReturnsAsync(hotel); 

            Func<Task> act = async () => await _guestReviewService.AddReviewAsync(hotelId, reviewRequest);
            await act.Should().ThrowAsync<UnauthorizedAccessException>();

            _hotelRepositoryMock.Verify(x => x.GetByIdAsync(hotelId), Times.Once);
        }



        [Fact]
        public async Task DeleteReviewAsync_ShouldThrowKeyNotFoundException_WhenReviewDoesNotExist()
        {
            var reviewId = 1;
            var hotelId = 1;

            _guestReviewRepositoryMock.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync((GuestReview)null);

            Func<Task> act = async () => await _guestReviewService.DeleteReviewAsync(hotelId, reviewId);
            await act.Should().ThrowAsync<KeyNotFoundException>();
            _guestReviewRepositoryMock.Verify(x => x.GetByIdAsync(reviewId), Times.Once);
        }

        [Fact]
        public async Task DeleteReviewAsync_ShouldThrowArgumentException_WhenHotelIdDoesNotMatch()
        {
            var reviewId = 1;
            var incorrectHotelId = 2;

            var review = new GuestReview { GuestReviewId = reviewId, HotelId = 1 };

            _guestReviewRepositoryMock.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync(review);

            Func<Task> act = async () => await _guestReviewService.DeleteReviewAsync(incorrectHotelId, reviewId);
            await act.Should().ThrowAsync<ArgumentException>();
            _guestReviewRepositoryMock.Verify(x => x.GetByIdAsync(reviewId), Times.Once);
        }




    }
}
