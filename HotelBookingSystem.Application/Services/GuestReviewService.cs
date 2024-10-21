using AutoMapper;
using HotelBookingSystem.Application.DTO.GuestReviewDTO;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace HotelBookingSystem.Application.Services
{
    public class GuestReviewService : IGuestReviewService
    {
        private readonly IGuestReviewRepository _guestReviewRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GuestReviewService(IGuestReviewRepository reviewRepository, IMapper mapper, IHotelRepository hotelRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _guestReviewRepository = reviewRepository;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<GuestReviewResponse> AddReviewAsync(int hotelId, GuestReviewRequest reviewRequest)
        {

            var hotel = await _hotelRepository.GetByIdAsync(hotelId);

            if (hotel == null)
                throw new KeyNotFoundException("Hotel not found");

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("User not found"));

            var review = _mapper.Map<GuestReview>(reviewRequest);
            review.HotelId = hotelId;

            review.UserId = userId;
            review.User = await _userRepository.GetByIdAsync(userId);

            await _guestReviewRepository.AddAsync(review);

            await UpdateHotelRating(hotel);

            return _mapper.Map<GuestReviewResponse>(review);
        }

        public async Task DeleteReviewAsync (int hotelId, int reviewId)
        {
            var review = await _guestReviewRepository.GetByIdAsync(reviewId);
            if (review == null)
                throw new KeyNotFoundException("Review not found");

            if (review.HotelId != hotelId)
                throw new ArgumentException("Invalid hotel ID");

            var hotel = await _hotelRepository.GetByIdAsync(review.HotelId);
            if (hotel == null)
                throw new KeyNotFoundException("Hotel not found");
            await _guestReviewRepository.DeleteAsync(reviewId);

            await UpdateHotelRating(hotel);

        }

        private async Task UpdateHotelRating(Hotel hotel)
        {
            var reviews = await _guestReviewRepository.GetReviewsByHotelIdAsync(hotel.HotelId);
            var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            if (hotel != null)
            {
                hotel.StarRating = (int)Math.Round(averageRating);
                await _hotelRepository.UpdateAsync(hotel);
            }
        }
    }
}
