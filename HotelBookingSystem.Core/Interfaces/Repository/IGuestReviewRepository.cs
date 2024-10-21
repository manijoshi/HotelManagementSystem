using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Domain.Interfaces.Repository
{
    public interface IGuestReviewRepository : IRepository<GuestReview>
    {
        Task<IEnumerable<GuestReview>> GetReviewsByHotelIdAsync(int hotelId);

    }
}
