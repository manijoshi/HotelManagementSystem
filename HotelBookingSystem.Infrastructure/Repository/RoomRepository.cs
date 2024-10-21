using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Repository
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Room> GetByIdAsync(int id)
        {
            var currentDate = DateTime.Now;

            return await _context.Rooms
            .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.RoomId == id);
        }

        public async Task<(IList<Room> Rooms, int TotalResults)> SearchAsync(IRoomSearchParameters searchParameters)
        {
            var query = _context.Rooms
                .Include(r => r.Bookings)
                .AsQueryable();


            if (searchParameters.CheckInDate != null && searchParameters.CheckOutDate != null)
            {
                query = query.Where(r => !r.Bookings.Any(b =>
                    b.CheckInDate < searchParameters.CheckOutDate && b.CheckOutDate > searchParameters.CheckInDate));
            }

            if (searchParameters.Adults.HasValue)
            {
                query = query.Where(r => r.AdultCapacity >= searchParameters.Adults);
            }

            if (searchParameters.Children.HasValue)
            {
                query = query.Where(r => r.ChildCapacity >= searchParameters.Children);
            }


            if (searchParameters.RoomTypes != null && searchParameters.RoomTypes.Any())
            {
                var roomTypesList = searchParameters.RoomTypes
                    .SelectMany(rt => rt.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()))
                    .ToList();

                var roomTypesEnums = roomTypesList
                    .Where(rt => Enum.TryParse(typeof(RoomType), rt, out _))
                    .Select(rt => (RoomType)Enum.Parse(typeof(RoomType), rt))
                    .ToList();

                query = query.Where(r => roomTypesEnums.Contains(r.RoomType));
            }

            if (searchParameters.MinPrice.HasValue)
            {
                query = query.Where(r => r.PricePerNight >= searchParameters.MinPrice);
            }

            if (searchParameters.MaxPrice.HasValue)
            {
                query = query.Where(r => r.PricePerNight <= searchParameters.MaxPrice);
            }

            var totalResults = await query.CountAsync();

            var rooms = await query
                .Skip((searchParameters.Page - 1) * searchParameters.PageSize)
                .Take(searchParameters.PageSize)
                .ToListAsync();

            return (rooms, totalResults);

        }
    }
}
