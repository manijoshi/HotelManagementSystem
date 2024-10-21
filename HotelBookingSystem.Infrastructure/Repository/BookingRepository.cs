using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Booking> GetByIdAsync(int bookingId)
        {
            return await _context.Bookings
                                 .Include(b => b.Payment)
                                 .Include(b => b.Hotel)
                                 .Include(b => b.User)
                                 .Include(b => b.Room)
                                 .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

    }

}
