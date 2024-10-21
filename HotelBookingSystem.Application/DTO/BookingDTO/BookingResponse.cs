
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using HotelBookingSystem.Application.DTO.PaymentDTO;
using HotelBookingSystem.Domain.Entities;


namespace HotelBookingSystem.Application.DTO.BookingDTO
{
    public class BookingResponse
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string SpecialRequests { get; set; }
        public double TotalPrice { get; set; }
        public PaymentResponse? Payment { get; set; }
    }

}
