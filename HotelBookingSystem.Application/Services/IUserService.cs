using HotelBookingSystem.Application.DTO.UserDTO;

namespace HotelBookingSystem.Application.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUserByIdAsync(int id);
        Task<UserResponse> RegisterUserAsync(UserRequest request);
        Task<string> AuthenticateUserAsync(UserLoginRequest request);
    }
}
