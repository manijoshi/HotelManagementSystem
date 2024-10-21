using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces;

namespace HotelBookingSystem.Application.Services
{
    public interface IHotelService
    {
        Task<HotelResponse> CreateHotelAsync(HotelRequest request);
        Task<HotelResponse> UpdateHotelAsync(int hotelId, HotelRequest request);
        Task DeleteHotelAsync(int hotelId);
        Task<HotelResponse> GetHotelByIdAsync(int hotelId);
        Task<HotelResponseWithReviews> GetHotelByIdWithReviewsAsync(int hotelId);
        Task<IPaginatedList<HotelResponse>> SearchHotelsAsync(HotelSearchParameters searchParameters);
        Task<IEnumerable<HotelResponse>> GetFeaturedDealsAsync(int limit);
        Task<IEnumerable<HotelResponse>> GetRecentHotelsAsync(int limit);
    }
}
