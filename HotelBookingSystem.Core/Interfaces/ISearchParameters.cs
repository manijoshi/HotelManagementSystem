

namespace HotelBookingSystem.Domain.Interfaces
{
    public interface ISearchParameters
    {
        string Query { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; } 
        int? Adults { get; set; }
        int? Children { get; set; }
        int? Rooms { get; set; }
        decimal? MinPrice { get; set; }
        decimal? MaxPrice { get; set; }
        int? MinRating { get; set; }
        int? MaxRating { get; set; }
        IList<string> Amenities { get; set; }
        IList<string> RoomTypes { get; set; }
        string? HotelType { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
    }
}
