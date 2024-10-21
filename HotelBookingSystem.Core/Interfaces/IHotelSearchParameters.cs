

namespace HotelBookingSystem.Domain.Interfaces
{
    public interface IHotelSearchParameters
    {
        public string Query { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public IList<string> Amenities { get; set; }
        public string? HotelType { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
