

namespace HotelBookingSystem.Domain.Entities
{
    public class GuestReview
    {
        public int GuestReviewId { get; set; }
        public int UserId { get; set; } 
        public int HotelId { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; } 
        public User User { get; set; } 
        public Hotel Hotel { get; set; } 
    }
}
