using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;
using HotelBookingSystem.Domain.Interfaces;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        public HotelRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IList<Hotel> Hotels, int TotalResults)> SearchAsync(IHotelSearchParameters searchParameters)
        {
            var query = _context.Hotels
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchParameters.Query))
            {
                query = query.Where(h => h.Name.Contains(searchParameters.Query) ||
                                          h.City.Name.Contains(searchParameters.Query));
            }

            if (searchParameters.MinRating.HasValue)
            {
                query = query.Where(h => h.StarRating >= searchParameters.MinRating);
            }

            if (searchParameters.MaxRating.HasValue)
            {
                query = query.Where(h => h.StarRating <= searchParameters.MaxRating);
            }

            if (searchParameters.Amenities != null && searchParameters.Amenities.Any())
            {
                var amenitiesList = searchParameters.Amenities
                    .SelectMany(a => a.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()))
                    .ToList();

                var amenitiesEnums = amenitiesList
                    .Where(a => Enum.TryParse(typeof(HotelAmenity), a, out _))
                    .Select(a => (HotelAmenity)Enum.Parse(typeof(HotelAmenity), a))
                    .ToList();

                query = query.Where(h => amenitiesEnums.All(a => h.Amenities.Contains(a)));
            }

            if (searchParameters.HotelType != null && searchParameters.HotelType.Any())
            {
                if (Enum.TryParse(searchParameters.HotelType, out HotelType hotelTypeEnum) && hotelTypeEnum != default)
                {
                    query = query.Where(h => h.HotelType == hotelTypeEnum);
                }
            }

            var totalResults = await query.CountAsync();

            var hotels = await query
                .Skip((searchParameters.Page - 1) * searchParameters.PageSize)
                .Take(searchParameters.PageSize)
                .ToListAsync();


            return (hotels, totalResults);

        }



        public async Task<Hotel> GetByIdWithReviewsAsync(int id)
        {
            var result = await _context.Hotels
                .Include(h => h.GuestReviews)
                .FirstOrDefaultAsync(h => h.HotelId == id);
            return result;
        }

        public async Task<IEnumerable<Hotel>> GetFeaturedDealsAsync(int limit)
        {
            var hotelsWithFeaturedDeals = await _context.Hotels
                .Include(h => h.Rooms)
                .Where(h => h.Rooms.Any(r => r.FeaturedDeal))
                .Select(h => new
                {
                    Hotel = h,
                    MinDiscountedPrice = h.Rooms
                        .Where(r => r.FeaturedDeal)
                        .Select(r => r.DiscountedPrice ?? double.MaxValue)
                        .Min()
                })
                .OrderBy(x => x.MinDiscountedPrice)
                .Take(limit)
                .ToListAsync();

            var sortedHotels = hotelsWithFeaturedDeals
                .Select(x => x.Hotel)
                .ToList();

            return sortedHotels;
        }

        public async Task<IEnumerable<Hotel>> GetRecientHotelsAsync(int userId, int limit)
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Hotels
                     .Where(h => h.Bookings.Any(b => b.UserId == userId && b.CheckOutDate < currentDate))
                     .OrderByDescending(h => h.Bookings
                         .Where(b => b.UserId == userId)
                         .Max(b => b.CheckOutDate))
                     .Distinct()
                     .Take(limit)
                     .ToListAsync();
        }
    }

}
