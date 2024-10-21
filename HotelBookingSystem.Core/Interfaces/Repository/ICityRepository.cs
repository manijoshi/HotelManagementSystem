using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Domain.Interfaces.Repository
{
    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetPopularCitiesAsync(int limit);
        Task<City> GetByIdWithHotelsAsync(int id);
    }
}
