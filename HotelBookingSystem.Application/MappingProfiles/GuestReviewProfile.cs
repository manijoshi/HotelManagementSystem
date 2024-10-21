

using AutoMapper;
using HotelBookingSystem.Application.DTO.GuestReviewDTO;
using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.MappingProfiles
{
    public class GuestReviewProfile : Profile
    {
        public GuestReviewProfile() 
        {
            CreateMap<GuestReviewRequest, GuestReview>();
            CreateMap<GuestReview, GuestReviewResponse>();
        }
    }
}
