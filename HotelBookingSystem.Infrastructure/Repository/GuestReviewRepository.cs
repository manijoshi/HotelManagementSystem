using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Repository
{
    public class GuestReviewRepository : Repository<GuestReview>, IGuestReviewRepository
    {
        public GuestReviewRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<GuestReview>> GetReviewsByHotelIdAsync(int hotelId) // This is used for calculating hotel star rating
        {
            return await _context.GuestReviews
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();
        }
    }

}
