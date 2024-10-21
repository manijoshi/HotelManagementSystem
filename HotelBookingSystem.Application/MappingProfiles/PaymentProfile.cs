

using AutoMapper;
using HotelBookingSystem.Application.DTO.PaymentDTO;
using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.MappingProfiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile() 
        {
            CreateMap<PaymentRequest, Payment>();
            CreateMap<Payment, PaymentResponse>();
        }
    }
}
