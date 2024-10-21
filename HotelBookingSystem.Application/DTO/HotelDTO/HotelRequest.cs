

namespace HotelBookingSystem.Application.DTO.HotelDTO
{
    public class HotelRequest
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Address { get; set; }
        public string HotelType { get; set; }
        public int CityId { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public IList<string> Amenities { get; set; }

    }
}
