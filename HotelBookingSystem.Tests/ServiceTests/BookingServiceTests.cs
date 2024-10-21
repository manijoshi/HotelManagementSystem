using AutoMapper;
using FluentAssertions;
using HotelBookingSystem.Application.DTO.BookingDTO;
using HotelBookingSystem.Application.DTO.PaymentDTO;
using HotelBookingSystem.Application.MappingProfiles;
using HotelBookingSystem.Application.Services;
using HotelBookingSystem.Application.Utilities;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Infrastructure.EmailSender;
using HotelBookingSystem.Infrastructure.PdfGen;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace HotelBookingSystem.Tests.ServiceTests
{
    public class BookingServiceTests
    {
        private readonly BookingService _bookingService;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IBookingPdfGenerator> _bookingPdfGenerator;
        private readonly Mock<IBookingEmailGenerator> _bookingEmailGeneratorMock;

        public BookingServiceTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _cityRepositoryMock = new Mock<ICityRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _bookingPdfGenerator = new Mock<IBookingPdfGenerator>();
            _bookingEmailGeneratorMock = new Mock<IBookingEmailGenerator>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HotelProfile>();
                cfg.AddProfile<RoomProfile>();
                cfg.AddProfile<PaymentProfile>();
                cfg.AddProfile<BookingProfile>();
            });

            _mapper = config.CreateMapper();
            _bookingService = new BookingService(
                _bookingRepositoryMock.Object,
                _roomRepositoryMock.Object,
                _hotelRepositoryMock.Object,
                _userRepositoryMock.Object,
                _mapper,
                _httpContextAccessorMock.Object,
                _cityRepositoryMock.Object,
                _emailServiceMock.Object,
                _bookingPdfGenerator.Object,
                _bookingEmailGeneratorMock.Object
            );
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns((Claim)null);

            var request = new BookingRequest { RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1) };

            await _bookingService.Invoking(service => service.CreateBookingAsync(request))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldThrowKeyNotFoundException_WhenRoomNotFound()
        {
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            _roomRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Room)null);

            var request = new BookingRequest { RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1) };

            await _bookingService.Invoking(service => service.CreateBookingAsync(request))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldThrowKeyNotFoundException_WhenHotelNotFound()
        {
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            _roomRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Room { Bookings = new List<Booking>() });
            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Hotel)null);

            var request = new BookingRequest { RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1) };

            await _bookingService.Invoking(service => service.CreateBookingAsync(request))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldThrowInvalidOperationException_WhenRoomNotAvailable()
        {
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var room = new Room { Bookings = new List<Booking> { new Booking { CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1) } } };
            var hotel = new Hotel();

            _roomRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(room);
            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(hotel);

            var request = new BookingRequest { RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(2) };

            await _bookingService.Invoking(service => service.CreateBookingAsync(request))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldReturnBookingResponse_WhenBookingIsSuccessful()
        {
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var city = new City
            {
                CityId = 1,
                Name = "Test City",
            };

            var hotel = new Hotel
            {
                HotelId = 1,
                Name = "Test Hotel",

            };
            var room = new Room
            {
                RoomId = 1,
                HotelId = 1,
                Bookings = new List<Booking>(),

            };
            var user = new User
            {
                UserId = 1,
                Email = "user@example.com",
                FirstName = "John",
                LastName = "Doe",
            };


            _roomRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(room);
            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(hotel);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _cityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(city);
            _emailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var request = new BookingRequest { RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1) };

            var result = await _bookingService.CreateBookingAsync(request);

            result.Should().BeOfType<BookingResponse>();
            city.Visitors.Should().Be(1);
        }

        [Fact]
        public async Task CancelBookingAsync_ShouldThrowKeyNotFoundException_WhenBookingNotFound()
        {
            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Booking)null);

            await _bookingService.Invoking(service => service.CancelBookingAsync(1))
                .Should().ThrowAsync<KeyNotFoundException>();
        }


        [Fact]
        public async Task GetBookingDetailsAsync_ShouldThrowKeyNotFoundException_WhenBookingNotFound()
        {
            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Booking)null);

            await _bookingService.Invoking(service => service.GetBookingDetailsAsync(1))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetBookingPdfAsync_ShouldThrowKeyNotFoundException_WhenBookingNotFound()
        {
            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Booking)null);

            await _bookingService.Invoking(service => service.GetBookingPdfAsync(1))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetBookingPdfAsync_ShouldReturnPdfBytes_WhenBookingExists()
        {
            var bookingResponse = new BookingResponse
            {
                BookingId = 1,
                UserFirstName = "John",
                UserLastName = "Doe",
                HotelName = "Example Hotel",
                HotelAddress = "123 Example Street",
                RoomType = "Deluxe",
                SpecialRequests = "None",
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = DateTime.UtcNow.AddDays(3),
                TotalPrice = 300,
                Payment = new PaymentResponse
                {
                    PaymentId = 1,
                    Amount = 300,
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethod = "Credit Card",
                    Status = "Completed"
                }
            };

            var booking = new Booking
            {
                BookingId = bookingResponse.BookingId,
            };

            var pdfBytes = new byte[] { 1, 2, 3, 4, 5 };

            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(booking.BookingId))
                .ReturnsAsync(booking);

            _bookingPdfGenerator.Setup(x => x.GeneratePdfAsync(It.IsAny<Booking>()))
                .ReturnsAsync(pdfBytes);

            var result = await _bookingService.GetBookingPdfAsync(booking.BookingId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(pdfBytes);
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldCalculateTotalPriceCorrectly()
        {
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var room = new Room
            {
                RoomId = 1,
                HotelId = 1,
                PricePerNight = 100,
                FeaturedDeal = true,
                DiscountedPrice = 80,
                Bookings = new List<Booking>(),
            };

            var hotel = new Hotel
            {
                HotelId = 1,
                Name = "Test Hotel",
            };

            var user = new User
            {
                UserId = 1,
                Email = "user@example.com",
            };

            _roomRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(room);
            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(hotel);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _cityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new City { CityId = 1 });

            var request = new BookingRequest { RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(3) };

            var result = await _bookingService.CreateBookingAsync(request);

            result.TotalPrice.Should().Be(240); 
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldSendEmailWithBookingDetails()
        {
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var city = new City
            {
                CityId = 1,
                Name = "Test City",
            };

            var hotel = new Hotel
            {
                HotelId = 1,
                Name = "Test Hotel",
            };

            var room = new Room
            {
                RoomId = 1,
                HotelId = 1,
                Bookings = new List<Booking>(),
            };

            var user = new User
            {
                UserId = 1,
                Email = "user@example.com",
            };

            _roomRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(room);
            _hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(hotel);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _cityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(city);

            _emailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var request = new BookingRequest { RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1) };

            await _bookingService.CreateBookingAsync(request);

            _emailServiceMock.Verify(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CancelBookingAsync_ShouldSubtractCityVisitors_WhenBookingIsCanceled()
        {
            var city = new City
            {
                CityId = 1,
                Visitors = 5,
            };

            var hotel = new Hotel
            {
                HotelId = 1,
                CityId = 1,
            };

            var booking = new Booking
            {
                BookingId = 1,
                Hotel = hotel,
            };

            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(booking);
            _cityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(city);

            await _bookingService.CancelBookingAsync(1);

            city.Visitors.Should().Be(4); 
        }

        [Fact]
        public async Task GetBookingDetailsAsync_ShouldReturnBookingResponse_WhenBookingExists()
        {
            var booking = new Booking
            {
                BookingId = 1,
                User = new User { FirstName = "John", LastName = "Doe" },
                Hotel = new Hotel { Name = "Test Hotel" },
                Room = new Room { RoomType = RoomType.Deluxe },
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = DateTime.UtcNow.AddDays(3),
                TotalPrice = 300,
            };

            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(booking);

            var result = await _bookingService.GetBookingDetailsAsync(1);

            result.Should().BeOfType<BookingResponse>();
            result.UserFirstName.Should().Be("John");
            result.HotelName.Should().Be("Test Hotel");
        }

        [Fact]
        public async Task GetBookingPdfAsync_ShouldGeneratePdf_WhenBookingExists()
        {
            var booking = new Booking
            {
                BookingId = 1,
            };

            var pdfBytes = new byte[] { 1, 2, 3, 4, 5 };

            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(booking);
            _bookingPdfGenerator.Setup(x => x.GeneratePdfAsync(It.IsAny<Booking>())).ReturnsAsync(pdfBytes);

            var result = await _bookingService.GetBookingPdfAsync(1);

            result.Should().BeEquivalentTo(pdfBytes);
        }


    }


}

