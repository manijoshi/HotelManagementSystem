

namespace HotelBookingSystem.Application.DTO.GuestReviewDTO
{
    public class GuestReviewResponse
    {
        public int GuestReviewId { get; set; }
        public int UserId { get; set; }
        public int HotelId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
