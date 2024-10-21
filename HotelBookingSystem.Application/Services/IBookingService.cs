

using HotelBookingSystem.Application.DTO.BookingDTO;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace HotelBookingSystem.Application.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(BookingRequest bookingRequest);
        Task CancelBookingAsync(int bookingId);
        Task<BookingResponse> GetBookingDetailsAsync(int bookingId);
        Task<byte[]> GetBookingPdfAsync(int bookingId);
    }
}
