

namespace HotelBookingSystem.Domain.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public string? SpecialRequests { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalPrice { get; set; }
        public User User { get; set; }
        public Room Room { get; set; }
        public Hotel Hotel { get; set; }

        public Payment Payment { get; set; }
    }

}
