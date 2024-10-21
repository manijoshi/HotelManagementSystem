using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<GuestReview> GuestReviews { get; set; }
    }

}
