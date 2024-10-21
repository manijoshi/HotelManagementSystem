using HotelBookingSystem.Domain.Entities;


namespace HotelBookingSystem.Application.DTO.PaymentDTO
{
    public class PaymentRequest
    {
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
