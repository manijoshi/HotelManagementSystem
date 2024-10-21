using AutoMapper;
using HotelBookingSystem.Application.DTO.UserDTO;
using HotelBookingSystem.Application.PasswordHasher;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using Microsoft.Extensions.Configuration;


namespace HotelBookingSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, IConfiguration configuration, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> RegisterUserAsync(UserRequest request)
        {

            if (await _userRepository.GetByEmailAsync(request.Email) != null)
                throw new InvalidOperationException("Email already in use");

            var user = _mapper.Map<User>(request);
            var salt = _passwordHasher.GenerateSalt();
            user.PasswordHash = _passwordHasher.HashPassword(request.Password, salt);

            await _userRepository.AddAsync(user);

            return _mapper.Map<UserResponse>(user);

        }

        public async Task<string> AuthenticateUserAsync(UserLoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return _tokenService.GenerateToken(user);
        }

    }
}