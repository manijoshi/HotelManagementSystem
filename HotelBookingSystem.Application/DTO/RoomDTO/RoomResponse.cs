

using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Application.DTO.RoomDTO
{
    public class RoomResponse
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public string RoomType { get; set; }
        public double PricePerNight { get; set; }
        public bool FeaturedDeal { get; set; }
        public double? DiscountedPrice { get; set; }
        public int AdultCapacity { get; set; }
        public int ChildCapacity { get; set; }
        public IList<string> ImagesUrl { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}
