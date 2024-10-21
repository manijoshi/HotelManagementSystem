namespace HotelBookingSystem.Infrastructure.PdfGen
{
    public interface IPdfGenerator<T>
    {
        Task<byte[]> GeneratePdfAsync(T data);
    }
}
