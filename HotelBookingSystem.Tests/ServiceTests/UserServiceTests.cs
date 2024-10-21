using AutoMapper;
using FluentAssertions;
using HotelBookingSystem.Application.DTO.UserDTO;
using HotelBookingSystem.Application.MappingProfiles;
using HotelBookingSystem.Application.PasswordHasher;
using HotelBookingSystem.Application.Services;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;
using HotelBookingSystem.Domain.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HotelBookingSystem.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });

            _mapper = config.CreateMapper();

            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _configurationMock = new Mock<IConfiguration>();
            _tokenServiceMock = new Mock<ITokenService>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _mapper,
                _passwordHasherMock.Object,
                _configurationMock.Object,
                _tokenServiceMock.Object
            );
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUserResponse_WhenUserExists()
        {
            var userId = 1;
            var user = new User
            {
                UserId = userId,
                Email = "user@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(userId);

            result.Should().NotBeNull();
            result.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
        {
            var userId = 1;

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            Func<Task> act = async () => await _userService.GetUserByIdAsync(userId);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("User not found");
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowInvalidOperationException_WhenEmailAlreadyInUse()
        {
            var request = new UserRequest { Email = "user@example.com" };
            var existingUser = new User { Email = "user@example.com" };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync(existingUser);

            Func<Task> act = async () => await _userService.RegisterUserAsync(request);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Email already in use");
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnUserResponse_WhenEmailIsNotInUse()
        {
            var request = new UserRequest
            {
                Email = "user@example.com",
                Password = "password"
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync((User)null);

            var salt = new byte[] { 1, 2, 3, 4, 5 };
            var hashedPassword = new byte[] { 6, 7, 8, 9, 0 };


            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User)null);

            var result = await _userService.RegisterUserAsync(request);

            result.Should().NotBeNull();
            result.Email.Should().Be(request.Email);
        }


        [Fact]
        public async Task AuthenticateUserAsync_ShouldThrowUnauthorizedAccessException_WhenInvalidCredentials()
        {
            var request = new UserLoginRequest
            {
                Email = "user@example.com",
                Password = "password"
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync((User)null);

            Func<Task> act = async () => await _userService.AuthenticateUserAsync(request);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid credentials");
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnToken_WhenValidCredentials()
        {
            var request = new UserLoginRequest
            {
                Email = "user@example.com",
                Password = "password"
            };

            var user = new User
            {
                UserId = 1,
                Email = request.Email,
                PasswordHash = "hashedPassword",
                Role = UserRole.Customer
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash))
                .Returns(true);

            _configurationMock.Setup(x => x["Jwt:Key"]).Returns("supersecretkey12345supersecretkey12345");
            _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("issuer");
            _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("audience");
            _tokenServiceMock.Setup(x => x.GenerateToken(user)).Returns("mockedToken");

            var token = await _userService.AuthenticateUserAsync(request);

            token.Should().NotBeNullOrEmpty();
        }
    }
}
