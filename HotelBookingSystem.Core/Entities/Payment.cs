

using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }=DateTime.UtcNow;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public Booking Booking { get; set; }
    }

}
