

using HotelBookingSystem.Application.DTO.GuestReviewDTO;

namespace HotelBookingSystem.Application.Services
{
    public interface IGuestReviewService
    {
        Task<GuestReviewResponse> AddReviewAsync(int hotelId, GuestReviewRequest reviewRequest);
        Task DeleteReviewAsync(int hotelId, int reviewId);

    }
}
