using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
