

using HotelBookingSystem.Domain.Interfaces.Repository;

namespace HotelBookingSystem.Application.DTO.RoomDTO
{
    public class RoomSearchParameters : IRoomSearchParameters
    {
        public DateTime? CheckInDate { get; set; } = DateTime.Today;
        public DateTime? CheckOutDate { get; set; } = DateTime.Today;
        public int? Adults { get; set; } = 2;
        public int? Children { get; set; } = 0;
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public IList<string>? RoomTypes { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
