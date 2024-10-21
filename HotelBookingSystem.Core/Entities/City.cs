

namespace HotelBookingSystem.Domain.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string PostOffice { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Visitors { get; set; }
    }

}
