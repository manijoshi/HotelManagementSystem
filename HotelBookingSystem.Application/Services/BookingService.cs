using AutoMapper;
using HotelBookingSystem.Application.DTO.BookingDTO;
using HotelBookingSystem.Application.Utilities;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Infrastructure.EmailSender;
using HotelBookingSystem.Infrastructure.PdfGen;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace HotelBookingSystem.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IBookingPdfGenerator _bookingPdfGenerator;
        private readonly IBookingEmailGenerator _bookingEmailGenerator;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IHotelRepository hotelRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ICityRepository cityRepository,
            IEmailService emailService,
            IBookingPdfGenerator bookingPdfGenerator,
            IBookingEmailGenerator bookingEmailGenerator
            )
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cityRepository = cityRepository;
            _emailService = emailService;
            _bookingPdfGenerator = bookingPdfGenerator;
            _bookingEmailGenerator = bookingEmailGenerator;
        }

        public async Task<BookingResponse> CreateBookingAsync(BookingRequest bookingRequest)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("User not found"));

            var room = await _roomRepository.GetByIdAsync(bookingRequest.RoomId);
            if (room == null) throw new KeyNotFoundException("Room not found");

            var hotel = await _hotelRepository.GetByIdAsync(room.HotelId);
            if (hotel == null) throw new KeyNotFoundException("Hotel not found");

            CalculateRoomAvailability
                (room, bookingRequest);

            double totalPrice = CalculateTotalPrice(room, bookingRequest);

            var booking = _mapper.Map<Booking>(bookingRequest);
            booking.HotelId = room.HotelId;

            booking.TotalPrice = totalPrice;
            booking.UserId = userId;
            booking.User = await _userRepository.GetByIdAsync(userId);

            await _bookingRepository.AddAsync(booking);
            await AddCityVisitors(hotel);

            var bookingResponse = _mapper.Map<BookingResponse>(booking);

            

            return bookingResponse;
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null) throw new KeyNotFoundException("Booking not found");
            if (booking.Payment != null) throw new InvalidOperationException("Cannot delete booking with an associated payment.");

            var city = await _cityRepository.GetByIdAsync(booking.Hotel.CityId);
            await SubstractCityVisitors(city);
            _bookingRepository.DeleteAsync(bookingId);
        }

        public async Task<BookingResponse> GetBookingDetailsAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null) throw new KeyNotFoundException("Booking not found");

            var bookingResponse = _mapper.Map<BookingResponse>(booking);

            return bookingResponse;
        }
        private static void CalculateRoomAvailability(Room room, BookingRequest bookingRequest)
        {
            var checkInDate = bookingRequest.CheckInDate;
            var checkOutDate = bookingRequest.CheckOutDate;

            var isRoomAvailable = room.Bookings.All(b => b.CheckOutDate <= checkInDate || b.CheckInDate >= checkOutDate);
            if (!isRoomAvailable) throw new InvalidOperationException("Room is already booked for the specified dates");
        }

        private async Task AddCityVisitors(Hotel hotel)
        {
            var city = await _cityRepository.GetByIdAsync(hotel.CityId);
            if (city != null)
            {
                city.Visitors++;
                await _cityRepository.UpdateAsync(city);
            }
        }

        private void SendEmailWithBookingDetails(Booking booking, BookingResponse bookingResponse)
        {
            string emailSubject = "Your booking details";
            string emailBody = _bookingEmailGenerator.GenerateBookingEmailBody(bookingResponse);
            _emailService.SendEmailAsync(booking.User.Email, emailSubject, emailBody);
        }

        private static double CalculateTotalPrice(Room room, BookingRequest bookingRequest)
        {
            var checkInDate = bookingRequest.CheckInDate;
            var checkOutDate = bookingRequest.CheckOutDate;

            var totalNights = (checkOutDate - checkInDate).Days;
            var pricePerNight = room.FeaturedDeal && room.DiscountedPrice.HasValue
                ? room.DiscountedPrice.Value
                : room.PricePerNight;
            var totalPrice = totalNights * pricePerNight;
            return totalPrice;
        }

        private async Task SubstractCityVisitors(City city)
        {
            if (city != null)
            {
                city.Visitors--;
                await _cityRepository.UpdateAsync(city);
            }
        }

        public async Task<byte[]> GetBookingPdfAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null) throw new KeyNotFoundException("Booking not found");

            return await _bookingPdfGenerator.GeneratePdfAsync(booking);
        }


    }

}
