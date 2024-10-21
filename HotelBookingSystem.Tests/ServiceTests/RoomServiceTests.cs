using AutoMapper;
using FluentAssertions;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Application.DTO.RoomDTO;
using HotelBookingSystem.Application.MappingProfiles;
using HotelBookingSystem.Application.Services;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces.Repository;
using Moq;

namespace HotelBookingSystem.Tests.ServiceTests
{
    public class RoomServiceTests
    {
        private readonly RoomService _roomService;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly IMapper _mapper;

        public RoomServiceTests()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomProfile>();
                cfg.AddProfile<HotelProfile>();
            });
            _mapper = configuration.CreateMapper();

            _roomService = new RoomService(_roomRepositoryMock.Object, _hotelRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task CreateRoomAsync_ShouldReturnRoomResponse()
        {
            var hotelId = 1;
            var request = new RoomRequest
            {
            };
            var room = new Room { RoomId = 1, HotelId = hotelId };
            var roomResponse = _mapper.Map<RoomResponse>(room);

            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(hotelId)).ReturnsAsync(true);
            _roomRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Room>())).ReturnsAsync(room);

            var result = await _roomService.CreateRoomAsync(hotelId, request);

            result.Should().BeEquivalentTo(roomResponse);
        }

        [Fact]
        public async Task CreateRoomAsync_HotelNotFound_ShouldThrowKeyNotFoundException()
        {
            var hotelId = 1;
            var request = new RoomRequest { };

            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(hotelId)).ReturnsAsync(false);

            Func<Task> act = async () => await _roomService.CreateRoomAsync(hotelId, request);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetRoomByIdAsync_InvalidHotelId_ShouldThrowArgumentException()
        {
            var roomId = 1;
            var hotelId = 1;
            var room = new Room { RoomId = roomId, HotelId = 2 };
            var roomResponse = _mapper.Map<RoomResponse>(room);

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId)).ReturnsAsync(room);

            Func<Task> act = async () => await _roomService.GetRoomByIdAsync(hotelId, roomId);

            await act.Should().ThrowAsync<ArgumentException>();
        }



        [Fact]
        public async Task UpdateRoomAsync_ShouldReturnUpdatedRoomResponse()
        {
            var roomId = 1;
            var hotelId = 1;
            var request = new RoomRequest
            {
            };
            var room = new Room { RoomId = roomId, HotelId = hotelId };
            var updatedRoom = new Room { RoomId = roomId, HotelId = hotelId };
            var roomResponse = _mapper.Map<RoomResponse>(updatedRoom);

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId)).ReturnsAsync(room);
            _mapper.Map(request, room);  
            _roomRepositoryMock.Setup(repo => repo.UpdateAsync(room)).ReturnsAsync(updatedRoom);

            var result = await _roomService.UpdateRoomAsync(hotelId, roomId, request);

            result.Should().BeEquivalentTo(roomResponse);
        }

        [Fact]
        public async Task UpdateRoomAsync_InvalidHotelId_ShouldThrowArgumentException()
        {
            var roomId = 1;
            var hotelId = 1;
            var request = new RoomRequest { };
            var room = new Room { RoomId = roomId, HotelId = 2 };

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId)).ReturnsAsync(room);

            Func<Task> act = async () => await _roomService.UpdateRoomAsync(hotelId, roomId, request);

            await act.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task UpdateRoomAsync_RoomNotFound_ShouldThrowKeyNotFoundException()
        {
            var roomId = 1;
            var hotelId = 1;
            var request = new RoomRequest { };

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId)).ReturnsAsync((Room)null);

            Func<Task> act = async () => await _roomService.UpdateRoomAsync(hotelId, roomId, request);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }


        [Fact]
        public async Task DeleteRoomAsync_ShouldCallDeleteAsync()
        {
            var roomId = 1;
            var hotelId = 1;
            var room = new Room { RoomId = roomId, HotelId = hotelId };
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId)).ReturnsAsync(room);

            await _roomService.DeleteRoomAsync(hotelId, roomId);

            _roomRepositoryMock.Verify(repo => repo.DeleteAsync(roomId), Times.Once);
        }

        [Fact]
        public async Task DeleteRoomAsync_InvalidHotelId_ShouldThrowArgumentException()
        {
            var roomId = 1;
            var hotelId = 1;
            var room = new Room { RoomId = roomId, HotelId = 2 };

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId)).ReturnsAsync(room);

            Func<Task> act = async () => await _roomService.DeleteRoomAsync(hotelId, roomId);

            await act.Should().ThrowAsync<ArgumentException>();
        }



        [Fact]
        public async Task GetRoomByIdAsync_ShouldReturnRoomResponse()
        {
            var roomId = 1;
            var hotelId = 1;
            var room = new Room { RoomId = roomId, HotelId = hotelId };
            var roomResponse = _mapper.Map<RoomResponse>(room);

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId)).ReturnsAsync(room);

            var result = await _roomService.GetRoomByIdAsync(hotelId, roomId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(roomResponse);
        }


    }
}

