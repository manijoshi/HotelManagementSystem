using HotelBookingSystem.Application.DTO.GuestReviewDTO;
using HotelBookingSystem.Application.DTO.RoomDTO;

namespace HotelBookingSystem.Application.DTO.HotelDTO
{
    public class HotelResponseWithReviews
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Address { get; set; }
        public string HotelType { get; set; }
        public int CityId { get; set; }
        public ICollection<string> Amenities { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public ICollection<GuestReviewResponse> GuestReviews { get; set; }
    }
}
