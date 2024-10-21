using HotelBookingSystem.Application.DTO.BookingDTO;

namespace HotelBookingSystem.Application.Utilities
{
    public interface IBookingEmailGenerator
    {
        string GenerateBookingEmailBody(BookingResponse bookingResponse);
    }
}
