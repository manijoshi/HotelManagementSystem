using HotelBookingSystem.Domain.Entities;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HotelBookingSystem.Infrastructure.PdfGen
{
    public class BookingPdfGenerator : IBookingPdfGenerator
    {
        public async Task<byte[]> GeneratePdfAsync(Booking booking)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("Booking Details")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                document.Add(new Paragraph($"Booking number: {booking.BookingId}"));
                document.Add(new Paragraph($"User Name: {booking.User.FirstName} {booking.User.LastName}"));
                document.Add(new Paragraph($"Hotel Name: {booking.Hotel.Name}"));
                document.Add(new Paragraph($"Hotel Address: {booking.Hotel.Address}"));
                document.Add(new Paragraph($"Room Type: {booking.Room.RoomType}"));
                document.Add(new Paragraph($"Special Requests: {booking.SpecialRequests}"));
                document.Add(new Paragraph($"Check-in Date: {booking.CheckInDate.ToString("d")}"));
                document.Add(new Paragraph($"Check-out Date: {booking.CheckOutDate.ToString("d")}"));
                document.Add(new Paragraph($"Total Price: ${booking.TotalPrice}"));

                if (booking.Payment != null)
                {
                    document.Add(new Paragraph("Payment Details"));
                    document.Add(new Paragraph($"Payment number: {booking.Payment.PaymentId}"));
                    document.Add(new Paragraph($"Amount: ${booking.Payment.Amount}"));
                    document.Add(new Paragraph($"Payment Date: {booking.Payment.PaymentDate.ToString("d")}"));
                    document.Add(new Paragraph($"Payment Method: {booking.Payment.PaymentMethod}"));
                    document.Add(new Paragraph($"Payment Status: {booking.Payment.Status}"));
                }

                document.Close();

                return ms.ToArray();
            }
        }
    }
}
