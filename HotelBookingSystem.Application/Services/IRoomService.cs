using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Application.DTO.RoomDTO;
using HotelBookingSystem.Domain.Interfaces;

namespace HotelBookingSystem.Application.Services
{
    public interface IRoomService
    {
        Task<RoomResponse> CreateRoomAsync(int hotelId, RoomRequest request);
        Task<RoomResponse> UpdateRoomAsync(int hotelId, int roomId, RoomRequest request);
        Task DeleteRoomAsync(int hotelId, int roomId);
        Task<RoomResponse> GetRoomByIdAsync(int hotelId, int roomId);
        Task<IPaginatedList<RoomResponse>> SearchRoomsAsync(RoomSearchParameters roomSearchParameters);
    }
}
