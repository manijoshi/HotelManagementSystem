

using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.DTO.RoomDTO
{
    public class RoomRequest
    {
        public string RoomType { get; set; }
        public double PricePerNight { get; set; }
        public int AdultCapacity { get; set; }
        public int ChildCapacity { get; set; }
        public bool FeaturedDeal { get; set; }
        public double? DiscountedPrice { get; set; }
        public IList<string> ImagesUrl { get; set; }

    }
}
