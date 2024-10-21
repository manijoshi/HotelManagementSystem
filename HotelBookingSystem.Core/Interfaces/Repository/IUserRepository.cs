using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Domain.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
