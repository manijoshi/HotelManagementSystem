using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HotelBookingSystem.Infrastructure.PdfGen
{
    public class PdfGenerator<T> : IPdfGenerator<T>
    {
        public async Task<byte[]> GeneratePdfAsync(T data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("Generated PDF")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                document.Add(new Paragraph(data.ToString()));

                document.Close();

                return ms.ToArray();
            }
        }
    }
}
