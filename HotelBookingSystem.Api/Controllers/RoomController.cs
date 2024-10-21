using HotelBookingSystem.Application.DTO.RoomDTO;
using HotelBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("create-room/{hotelId}")]
        public async Task<IActionResult> CreateRoom(int hotelId, [FromBody] RoomRequest request)
        {
            var room = await _roomService.CreateRoomAsync(hotelId, request);
            return CreatedAtAction(nameof(GetRoomById), new { hotelId = room.HotelId, roomId = room.RoomId }, room);
        }

        [HttpGet("{hotelId}/{roomId}")]
        public async Task<IActionResult> GetRoomById(int hotelId, int roomId)
        {
            var room = await _roomService.GetRoomByIdAsync(hotelId, roomId);
            return Ok(room);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{hotelId}/{roomId}")]
        public async Task<IActionResult> UpdateRoom(int hotelId, int roomId, [FromBody] RoomRequest request)
        {
            var room = await _roomService.UpdateRoomAsync(hotelId, roomId, request);
            return Ok(room);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{hotelId}/{roomId}")]
        public async Task<IActionResult> DeleteRoom(int hotelId, int roomId)
        {
            await _roomService.DeleteRoomAsync(hotelId, roomId);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRooms([FromQuery] RoomSearchParameters searchParameters)
        {
            var rooms = await _roomService.SearchRoomsAsync(searchParameters);
            return Ok(rooms);
        }
    }
}
