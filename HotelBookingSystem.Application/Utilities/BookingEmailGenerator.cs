using HotelBookingSystem.Application.DTO.BookingDTO;
using System.Text;


namespace HotelBookingSystem.Application.Utilities
{
    public class BookingEmailGenerator : IBookingEmailGenerator
    {
        public string GenerateBookingEmailBody(BookingResponse bookingResponse)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<h1>Booking Details</h1>");
            sb.AppendLine($"<p><strong>Booking number:</strong> {bookingResponse.BookingId}</p>");
            sb.AppendLine($"<p><strong>User Name:</strong> {bookingResponse.UserFirstName} {bookingResponse.UserLastName}</p>");
            sb.AppendLine($"<p><strong>Hotel Name:</strong> {bookingResponse.HotelName}</p>");
            sb.AppendLine($"<p><strong>Hotel Address:</strong> {bookingResponse.HotelAddress}</p>");
            sb.AppendLine($"<p><strong>Room Type:</strong> {bookingResponse.RoomType}</p>");
            sb.AppendLine($"<p><strong>Special Requests:</strong> {bookingResponse.SpecialRequests}</p>");
            sb.AppendLine($"<p><strong>Check-in Date:</strong> {bookingResponse.CheckInDate.ToString("d")}</p>");
            sb.AppendLine($"<p><strong>Check-out Date:</strong> {bookingResponse.CheckOutDate.ToString("d")}</p>");
            sb.AppendLine($"<p><strong>Total Price:</strong> ${bookingResponse.TotalPrice}</p>");

            if (bookingResponse.Payment != null)
            {
                sb.AppendLine("<h2>Payment Details</h2>");
                sb.AppendLine($"<p><strong>Payment number:</strong> {bookingResponse.Payment.PaymentId}</p>");
                sb.AppendLine($"<p><strong>Amount:</strong> ${bookingResponse.Payment.Amount}</p>");
                sb.AppendLine($"<p><strong>Payment Date:</strong> {bookingResponse.Payment.PaymentDate.ToString("d")}</p>");
                sb.AppendLine($"<p><strong>Payment Method:</strong> {bookingResponse.Payment.PaymentMethod}</p>");
                sb.AppendLine($"<p><strong>Payment Status:</strong> {bookingResponse.Payment.Status}</p>");
            }

            return sb.ToString();
        }
    }
}
