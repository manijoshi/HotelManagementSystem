using AutoMapper;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Application.DTO.RoomDTO;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Interfaces;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Domain.Utilities;

namespace HotelBookingSystem.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<RoomResponse> CreateRoomAsync(int hotelId, RoomRequest request)
        {
            var room = _mapper.Map<Room>(request);
            if (!await _hotelRepository.ExistsAsync(hotelId))
            {
                throw new KeyNotFoundException("Hotel not found");
            }
            room.CreatedAt = DateTime.UtcNow;
            room.UpdatedAt = DateTime.UtcNow;
            room.HotelId = hotelId;

            var createdRoom = await _roomRepository.AddAsync(room);
            return _mapper.Map<RoomResponse>(createdRoom);
        }

        public async Task<RoomResponse> UpdateRoomAsync(int hotelId, int roomId, RoomRequest request)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException("Room not found");
            if (room.HotelId != hotelId)
            {
                throw new ArgumentException("Invalid hotel ID");
            }
            _mapper.Map(request, room);
            room.UpdatedAt = DateTime.UtcNow;
            room.HotelId = hotelId;

            var updatedRoom = await _roomRepository.UpdateAsync(room);
            return _mapper.Map<RoomResponse>(updatedRoom);
        }

        public async Task DeleteRoomAsync(int hotelId, int roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room.HotelId != hotelId)
            {
                throw new ArgumentException("Invalid hotel ID");
            }
            await _roomRepository.DeleteAsync(roomId);
        }

        public async Task<RoomResponse> GetRoomByIdAsync(int hotelId, int roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException("Room not found");
            if (room.HotelId != hotelId)
            {
                throw new ArgumentException("Invalid hotel ID");
            }
            return _mapper.Map<RoomResponse>(room);
        }

        public async Task<IPaginatedList<RoomResponse>> SearchRoomsAsync(RoomSearchParameters searchParameters)
        {
            var (rooms, totalResults) = await _roomRepository.SearchAsync(searchParameters);

            var totalPages = (int)Math.Ceiling(totalResults / (double)searchParameters.PageSize);
            var roomResponses = _mapper.Map<List<RoomResponse>>(rooms);

            var paginatedList = new PaginatedList<RoomResponse>
            {
                TotalResults = totalResults,
                CurrentPage = searchParameters.Page,
                PageSize = searchParameters.PageSize,
                TotalPages = totalPages,
                Items = roomResponses
            };

            return paginatedList;
        }

    }
}
