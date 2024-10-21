

using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Domain.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public RoomType RoomType { get; set; }
        public double PricePerNight { get; set; }
        public int AdultCapacity { get; set; }
        public int ChildCapacity { get; set; }
        public bool FeaturedDeal { get; set; } 
        public double? DiscountedPrice { get; set; }
        public Hotel Hotel { get; set; }
        public IList<string> ImagesUrl { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
