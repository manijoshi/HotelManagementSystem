using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Infrastructure.Repository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<City>> GetPopularCitiesAsync(int limit)
        {
            return await _context.Cities
                .OrderByDescending(c => c.Visitors)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<City> GetByIdWithHotelsAsync(int id)
        {
            return await _context.Cities
                .Include(c => c.Hotels)
                .FirstOrDefaultAsync(c => c.CityId == id);
        }
    }

}
