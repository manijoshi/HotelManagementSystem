

using HotelBookingSystem.Domain.Interfaces;

namespace HotelBookingSystem.Application.DTO.HotelDTO
{
    public class HotelSearchParameters : IHotelSearchParameters
    {
        public string? Query { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public IList<string>? Amenities { get; set; }
        public string? HotelType { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
