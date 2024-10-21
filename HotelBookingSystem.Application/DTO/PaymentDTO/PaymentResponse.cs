
using HotelBookingSystem.Application.DTO.BookingDTO;
using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.DTO.PaymentDTO
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
