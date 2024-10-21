using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.DTO.CityDTO
{
    public class CityResponse
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string PostOffice { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Visitors { get; set; }

    }
}
